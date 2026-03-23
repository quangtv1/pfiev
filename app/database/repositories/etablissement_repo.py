from app.database.config_db import ConfigDB
from app.models.etablissement import Etablissement


class EtablissementRepository:
    def __init__(self, use_session_db: bool = False):
        if use_session_db:
            from app.database.session_db import SessionDB
            self._conn = SessionDB.conn()
        else:
            self._conn = ConfigDB.conn()

    def get_all(self) -> list:
        cur = self._conn.execute("SELECT * FROM Etablissement ORDER BY EtabID")
        return [Etablissement(
            etab_id=r["EtabID"],
            etab_nom=r["EtabNom"],
            etab_code=r["EtabCode"] or ""
        ) for r in cur.fetchall()]

    def get_by_id(self, etab_id: int):
        row = self._conn.execute(
            "SELECT * FROM Etablissement WHERE EtabID=?", (etab_id,)
        ).fetchone()
        if not row:
            return None
        return Etablissement(row["EtabID"], row["EtabNom"], row["EtabCode"] or "")

    def add(self, e: Etablissement) -> int:
        cur = self._conn.execute(
            "INSERT INTO Etablissement (EtabNom, EtabCode) VALUES (?,?)",
            (e.etab_nom, e.etab_code)
        )
        self._conn.commit()
        return cur.lastrowid

    def update(self, e: Etablissement):
        self._conn.execute(
            "UPDATE Etablissement SET EtabNom=?, EtabCode=? WHERE EtabID=?",
            (e.etab_nom, e.etab_code, e.etab_id)
        )
        self._conn.commit()

    def delete(self, etab_id: int):
        self._conn.execute("DELETE FROM Etablissement WHERE EtabID=?", (etab_id,))
        self._conn.commit()
