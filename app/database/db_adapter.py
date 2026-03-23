"""Unified DB adapter: sqlite3 for .db/.mdb (SQLite), pyodbc for legacy Access .mdb files."""
import re
import sqlite3
from pathlib import Path
from typing import Optional


def _is_access_available() -> bool:
    try:
        import pyodbc
        return any("Access" in d for d in pyodbc.drivers())
    except ImportError:
        return False


def _is_real_access_file(path: str) -> bool:
    """Check magic bytes to confirm file is a genuine Access .mdb/.accdb (not SQLite)."""
    try:
        with open(path, "rb") as f:
            header = f.read(16)
        # SQLite magic: starts with b'SQLite format 3'
        if header[:15] == b"SQLite format 3":
            return False
        # Access JET magic: bytes 4-19 = b'Standard Jet DB' or b'Standard ACE DB'
        if header[4:15] in (b"Standard Je", b"Standard AC"):
            return True
        # Unknown format — treat as SQLite to be safe
        return False
    except (OSError, IOError):
        return False


def _access_driver() -> str:
    import pyodbc
    for d in pyodbc.drivers():
        if "Access" in d:
            return d
    raise RuntimeError("No Microsoft Access ODBC driver found")


def _fix_sql_access(sql: str) -> str:
    """Translate SQLite dialect → Access/JET SQL."""
    sql = re.sub(r"\s+NULLS\s+LAST", "", sql, flags=re.IGNORECASE)
    m = re.search(r"\bLIMIT\s+(\d+)\s*$", sql, re.IGNORECASE)
    if m:
        n = m.group(1)
        sql = re.sub(r"\bLIMIT\s+\d+\s*$", "", sql, re.IGNORECASE).strip()
        sql = re.sub(r"\bSELECT\b", f"SELECT TOP {n}", sql, count=1, flags=re.IGNORECASE)
    return sql


class _DictRow:
    """Row with both positional and column-name access, compatible with dict()."""
    def __init__(self, columns: list, values: tuple):
        self._d = dict(zip(columns, values))
        self._v = values

    def __getitem__(self, key):
        if isinstance(key, str):
            return self._d[key]
        return self._v[key]

    def keys(self):
        return self._d.keys()

    def __iter__(self):
        return iter(self._v)

    def __len__(self):
        return len(self._v)


class _AccessCursor:
    """Wraps a pyodbc cursor to return _DictRow objects."""
    def __init__(self, pyodbc_conn, pyodbc_cursor):
        self._connection = pyodbc_conn
        self._cur = pyodbc_cursor

    def _cols(self):
        if self._cur.description:
            return [d[0] for d in self._cur.description]
        return []

    def _wrap(self, row):
        if row is None:
            return None
        return _DictRow(self._cols(), tuple(row))

    def fetchone(self):
        return self._wrap(self._cur.fetchone())

    def fetchall(self):
        cols = self._cols()
        return [_DictRow(cols, tuple(r)) for r in self._cur.fetchall()]

    def __iter__(self):
        cols = self._cols()
        for row in self._cur:
            yield _DictRow(cols, tuple(row))

    @property
    def lastrowid(self):
        try:
            cur = self._connection.cursor()
            cur.execute("SELECT @@IDENTITY")
            row = cur.fetchone()
            return int(row[0]) if row and row[0] is not None else None
        except Exception:
            return None


class DbConnection:
    """Unified wrapper for sqlite3 or pyodbc (Access .mdb)."""

    def __init__(self, path: str, force_sqlite: bool = False):
        self._path = path
        ext = Path(path).suffix.lower()
        self._use_access = (
            (not force_sqlite)
            and (ext == ".mdb")
            and _is_access_available()
            and _is_real_access_file(path)
        )
        if self._use_access:
            import pyodbc
            drv = _access_driver()
            self._conn = pyodbc.connect(
                f"DRIVER={{{drv}}};DBQ={path};", autocommit=False
            )
        else:
            self._conn = sqlite3.connect(path, check_same_thread=False)
            self._conn.row_factory = sqlite3.Row
            try:
                self._conn.execute("PRAGMA foreign_keys = ON")
            except Exception:
                pass

    def execute(self, sql: str, params=()):
        if self._use_access:
            cur = self._conn.cursor()
            cur.execute(_fix_sql_access(sql), params)
            return _AccessCursor(self._conn, cur)
        return self._conn.execute(sql, params)

    def executescript(self, script: str):
        """Only supported for sqlite3 path (DDL creation)."""
        if not self._use_access:
            self._conn.executescript(script)

    def commit(self):
        self._conn.commit()

    def close(self):
        self._conn.close()

    @property
    def is_access(self) -> bool:
        return self._use_access


def open_db(path: str, force_sqlite: bool = False) -> DbConnection:
    """Open database: pyodbc for real Access .mdb (if driver available), else sqlite3."""
    return DbConnection(path, force_sqlite=force_sqlite)
