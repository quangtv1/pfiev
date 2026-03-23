from typing import Optional
from app.database.db_adapter import DbConnection, open_db


class SessionDB:
    _conn: Optional[DbConnection] = None
    _path: str = ""

    @classmethod
    def open(cls, path: str):
        cls._path = path
        cls._conn = open_db(path)

    @classmethod
    def conn(cls) -> DbConnection:
        if cls._conn is None:
            raise RuntimeError("SessionDB not open")
        return cls._conn

    @classmethod
    def close(cls):
        if cls._conn:
            cls._conn.close()
            cls._conn = None

    @classmethod
    def is_open(cls) -> bool:
        return cls._conn is not None
