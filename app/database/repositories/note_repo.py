from app.database.session_db import SessionDB
from app.models.note import Note


class NoteRepository:
    def __init__(self):
        self._conn = SessionDB.conn()

    def get_by_candidat(self, candidat_id: int) -> list:
        cur = self._conn.execute(
            "SELECT * FROM Note WHERE CandidatID=? ORDER BY MatiereID",
            (candidat_id,)
        )
        return [Note(r["NoteID"], r["CandidatID"], r["MatiereID"], r["Note"] or "0")
                for r in cur.fetchall()]

    def upsert(self, candidat_id: int, matiere_id: int, value: str):
        """Insert or update note for (candidat_id, matiere_id)."""
        existing = self._conn.execute(
            "SELECT NoteID FROM Note WHERE CandidatID=? AND MatiereID=?",
            (candidat_id, matiere_id)
        ).fetchone()
        if existing:
            self._conn.execute(
                "UPDATE Note SET Note=? WHERE NoteID=?",
                (value, existing[0])
            )
        else:
            self._conn.execute(
                "INSERT INTO Note (CandidatID, MatiereID, Note) VALUES (?,?,?)",
                (candidat_id, matiere_id, value)
            )
        self._conn.commit()

    def delete_by_candidat(self, candidat_id: int):
        self._conn.execute("DELETE FROM Note WHERE CandidatID=?", (candidat_id,))
        self._conn.commit()
