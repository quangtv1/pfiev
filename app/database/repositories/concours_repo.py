from typing import Optional
from app.database.session_db import SessionDB
from app.models.concours import Concours


class ConcoursRepository:
    def __init__(self):
        self._conn = SessionDB.conn()

    def get_params(self) -> Optional[Concours]:
        row = self._conn.execute(
            "SELECT * FROM Concours LIMIT 1"
        ).fetchone()
        if not row:
            return None
        return Concours(
            concours_id=row["ConcoursID"],
            annee=row["Annee"] or 0,
            moyenne_min=row["MoyenneMin"] or 0.0
        )

    def set_params(self, c: Concours):
        existing = self._conn.execute("SELECT ConcoursID FROM Concours LIMIT 1").fetchone()
        if existing:
            self._conn.execute(
                "UPDATE Concours SET Annee=?, MoyenneMin=? WHERE ConcoursID=?",
                (c.annee, c.moyenne_min, existing[0])
            )
        else:
            self._conn.execute(
                "INSERT INTO Concours (Annee, MoyenneMin) VALUES (?,?)",
                (c.annee, c.moyenne_min)
            )
        self._conn.commit()
