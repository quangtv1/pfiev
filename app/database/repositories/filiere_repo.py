from app.database.config_db import ConfigDB
from app.models.filiere import Filiere


class FiliereRepository:
    def __init__(self, use_session_db: bool = False):
        if use_session_db:
            from app.database.session_db import SessionDB
            self._conn = SessionDB.conn()
        else:
            self._conn = ConfigDB.conn()

    def get_all(self) -> list:
        cur = self._conn.execute("SELECT * FROM Filiere ORDER BY FiliereID")
        return [Filiere(
            filiere_id=r["FiliereID"],
            filiere_nom=r["FiliereNom"],
            filiere_code=r["FiliereCode"] or "",
            etab_id=r["EtabID"] or 0,
            nb_place=r["NbPlace"] or 0
        ) for r in cur.fetchall()]

    def get_all_with_etab(self) -> list:
        """Returns list of dicts with EtabNom joined."""
        sql = """
            SELECT f.*, e.EtabNom
            FROM Filiere f
            LEFT JOIN Etablissement e ON f.EtabID = e.EtabID
            ORDER BY f.FiliereID
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_by_id(self, filiere_id: int):
        row = self._conn.execute(
            "SELECT * FROM Filiere WHERE FiliereID=?", (filiere_id,)
        ).fetchone()
        if not row:
            return None
        return Filiere(row["FiliereID"], row["FiliereNom"],
                       row["FiliereCode"] or "", row["EtabID"] or 0, row["NbPlace"] or 0)

    def add(self, f: Filiere) -> int:
        cur = self._conn.execute(
            "INSERT INTO Filiere (FiliereNom, FiliereCode, EtabID, NbPlace) VALUES (?,?,?,?)",
            (f.filiere_nom, f.filiere_code, f.etab_id, f.nb_place)
        )
        self._conn.commit()
        return cur.lastrowid

    def update(self, f: Filiere):
        self._conn.execute(
            "UPDATE Filiere SET FiliereNom=?, FiliereCode=?, EtabID=?, NbPlace=? WHERE FiliereID=?",
            (f.filiere_nom, f.filiere_code, f.etab_id, f.nb_place, f.filiere_id)
        )
        self._conn.commit()

    def delete(self, filiere_id: int):
        self._conn.execute("DELETE FROM Filiere WHERE FiliereID=?", (filiere_id,))
        self._conn.commit()

    def update_nb_place(self, filiere_id: int, nb_place: int):
        self._conn.execute(
            "UPDATE Filiere SET NbPlace=? WHERE FiliereID=?", (nb_place, filiere_id)
        )
        self._conn.commit()

    def set_all_nb_place(self, nb_place: int):
        """Set same NbPlace for all filieres."""
        self._conn.execute("UPDATE Filiere SET NbPlace=?", (nb_place,))
        self._conn.commit()
