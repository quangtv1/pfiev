from app.database.config_db import ConfigDB


class DataPathRepository:
    def __init__(self):
        self._conn = ConfigDB.conn()

    def get_all(self) -> list:
        """Return list of file paths."""
        cur = self._conn.execute("SELECT FilePath FROM DataPath ORDER BY PathID")
        return [r[0] for r in cur.fetchall()]

    def add(self, file_path: str):
        # Avoid duplicates
        existing = self._conn.execute(
            "SELECT PathID FROM DataPath WHERE FilePath=?", (file_path,)
        ).fetchone()
        if not existing:
            self._conn.execute(
                "INSERT INTO DataPath (FilePath) VALUES (?)", (file_path,)
            )
            self._conn.commit()

    def remove(self, file_path: str):
        self._conn.execute("DELETE FROM DataPath WHERE FilePath=?", (file_path,))
        self._conn.commit()

    def clear_all(self):
        self._conn.execute("DELETE FROM DataPath")
        self._conn.commit()
