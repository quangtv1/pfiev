from app.database.session_db import SessionDB
from app.models.choix import Choix


class ChoixRepository:
    def __init__(self):
        self._conn = SessionDB.conn()

    def get_by_candidat(self, candidat_id: int) -> list:
        cur = self._conn.execute(
            "SELECT * FROM Choix WHERE CandidatID=? ORDER BY ChoixOrdre",
            (candidat_id,)
        )
        return [self._row_to_model(r) for r in cur.fetchall()]

    def get_by_filiere(self, filiere_id: int) -> list:
        cur = self._conn.execute(
            "SELECT * FROM Choix WHERE FiliereID=? ORDER BY ChoixOrdre",
            (filiere_id,)
        )
        return [self._row_to_model(r) for r in cur.fetchall()]

    def get_all(self) -> list:
        cur = self._conn.execute("SELECT * FROM Choix ORDER BY CandidatID, ChoixOrdre")
        return [self._row_to_model(r) for r in cur.fetchall()]

    def save_choices(self, candidat_id: int, filiere_ids: list):
        """Replace all choices for a candidat with ordered list."""
        self._conn.execute("DELETE FROM Choix WHERE CandidatID=?", (candidat_id,))
        for i, fid in enumerate(filiere_ids, 1):
            self._conn.execute(
                "INSERT INTO Choix (CandidatID, FiliereID, ChoixOrdre, ChoixAdmis) VALUES (?,?,?,0)",
                (candidat_id, fid, i)
            )
        self._conn.commit()

    def reset_admis(self):
        """Reset all ChoixAdmis flags before running attribution."""
        self._conn.execute("UPDATE Choix SET ChoixAdmis=0")
        self._conn.commit()

    def set_admis(self, candidat_id: int, filiere_id: int):
        """Mark one choice as admitted."""
        self._conn.execute(
            "UPDATE Choix SET ChoixAdmis=1 WHERE CandidatID=? AND FiliereID=?",
            (candidat_id, filiere_id)
        )
        self._conn.commit()

    def count_admis(self, filiere_id: int) -> int:
        row = self._conn.execute(
            "SELECT COUNT(*) FROM Choix WHERE FiliereID=? AND ChoixAdmis=1",
            (filiere_id,)
        ).fetchone()
        return row[0] if row else 0

    def get_admitted_filiere_id(self, candidat_id: int):
        row = self._conn.execute(
            "SELECT FiliereID FROM Choix WHERE CandidatID=? AND ChoixAdmis=1",
            (candidat_id,)
        ).fetchone()
        return row[0] if row else None

    def change_spe(self, candidat_id: int, old_filiere_id: int, new_filiere_id: int):
        """Change assigned filiere after attribution."""
        self._conn.execute(
            "UPDATE Choix SET ChoixAdmis=0 WHERE CandidatID=? AND FiliereID=?",
            (candidat_id, old_filiere_id)
        )
        self._conn.execute(
            "UPDATE Choix SET ChoixAdmis=1 WHERE CandidatID=? AND FiliereID=?",
            (candidat_id, new_filiere_id)
        )
        self._conn.commit()

    @staticmethod
    def _row_to_model(r) -> Choix:
        return Choix(
            choix_id=r["ChoixID"],
            candidat_id=r["CandidatID"],
            filiere_id=r["FiliereID"],
            choix_ordre=r["ChoixOrdre"] or 1,
            choix_admis=bool(r["ChoixAdmis"]),
            filiere_nb_place=r["FiliereNbPlace"] or 0
        )
