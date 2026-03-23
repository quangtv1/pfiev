from app.database.config_db import ConfigDB
from app.models.matiere import Matiere


class MatiereRepository:
    def __init__(self, use_session_db: bool = False):
        if use_session_db:
            from app.database.session_db import SessionDB
            self._conn = SessionDB.conn()
        else:
            self._conn = ConfigDB.conn()

    def get_all(self) -> list:
        cur = self._conn.execute("SELECT * FROM Matiere ORDER BY MatiereID")
        return [Matiere(
            matiere_id=r["MatiereID"],
            matiere_nom=r["MatiereNom"],
            matiere_coefficient=r["MatiereCoefficient"] or 1.0,
            matiere_code=r["MatiereCode"] or ""
        ) for r in cur.fetchall()]

    def get_by_id(self, matiere_id: int):
        row = self._conn.execute(
            "SELECT * FROM Matiere WHERE MatiereID=?", (matiere_id,)
        ).fetchone()
        if not row:
            return None
        return Matiere(row["MatiereID"], row["MatiereNom"],
                       row["MatiereCoefficient"] or 1.0, row["MatiereCode"] or "")

    def add(self, m: Matiere) -> int:
        cur = self._conn.execute(
            "INSERT INTO Matiere (MatiereNom, MatiereCoefficient, MatiereCode) VALUES (?,?,?)",
            (m.matiere_nom, m.matiere_coefficient, m.matiere_code)
        )
        self._conn.commit()
        return cur.lastrowid

    def update(self, m: Matiere):
        self._conn.execute(
            "UPDATE Matiere SET MatiereNom=?, MatiereCoefficient=?, MatiereCode=? WHERE MatiereID=?",
            (m.matiere_nom, m.matiere_coefficient, m.matiere_code, m.matiere_id)
        )
        self._conn.commit()

    def delete(self, matiere_id: int):
        self._conn.execute("DELETE FROM Matiere WHERE MatiereID=?", (matiere_id,))
        self._conn.commit()
