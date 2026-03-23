from typing import Optional
from app.database.session_db import SessionDB
from app.models.candidat import Candidat


class CandidatRepository:
    def __init__(self):
        self._conn = SessionDB.conn()

    def get_all(self) -> list:
        cur = self._conn.execute("SELECT * FROM Candidat ORDER BY CandidatID")
        return [self._row_to_model(r) for r in cur.fetchall()]

    def get_by_id(self, cid: int) -> Optional[Candidat]:
        row = self._conn.execute(
            "SELECT * FROM Candidat WHERE CandidatID=?", (cid,)
        ).fetchone()
        return self._row_to_model(row) if row else None

    def get_session_view(self) -> list:
        """JOIN Candidat + Etablissement — returns list of dicts for QTableWidget."""
        sql = """
            SELECT c.*, e.EtabNom
            FROM Candidat c
            LEFT JOIN Etablissement e ON c.EtabID = e.EtabID
            ORDER BY c.CandidatID
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def add(self, c: Candidat) -> int:
        cur = self._conn.execute(
            """INSERT INTO Candidat
               (Nom, NomIntermediaire, Prenom, DateDeNaissance, Sexe,
                CandidatStatut, Langue, EtabID, anonymat)
               VALUES (?,?,?,?,?,?,?,?,?)""",
            (c.nom, c.nom_intermediaire, c.prenom, c.date_de_naissance,
             c.sexe, c.candidat_statut, c.langue, c.etab_id, c.anonymat)
        )
        self._conn.commit()
        return cur.lastrowid

    def update(self, c: Candidat):
        self._conn.execute(
            """UPDATE Candidat SET
               Nom=?, NomIntermediaire=?, Prenom=?, DateDeNaissance=?, Sexe=?,
               CandidatStatut=?, Langue=?, EtabID=?, anonymat=?
               WHERE CandidatID=?""",
            (c.nom, c.nom_intermediaire, c.prenom, c.date_de_naissance,
             c.sexe, c.candidat_statut, c.langue, c.etab_id, c.anonymat, c.candidat_id)
        )
        self._conn.commit()

    def delete(self, cid: int):
        self._conn.execute("DELETE FROM Note WHERE CandidatID=?", (cid,))
        self._conn.execute("DELETE FROM Choix WHERE CandidatID=?", (cid,))
        self._conn.execute("DELETE FROM Candidat WHERE CandidatID=?", (cid,))
        self._conn.commit()

    def get_ranked_for_attribution(self) -> list:
        sql = """
            SELECT CandidatID FROM Candidat
            WHERE CandidatStatut = 'I'
               OR (CandidatStatut = 'E'
                   AND CandidatMoyenne > (SELECT MoyenneMin FROM Concours LIMIT 1))
            ORDER BY CandidatClassement
        """
        return [r[0] for r in self._conn.execute(sql).fetchall()]

    def compute_averages_and_ranking(self, config_conn):
        """Compute weighted average then assign classement rank."""
        # Load coefficients from config DB
        coeffs = {r[0]: r[1] for r in
                  config_conn.execute("SELECT MatiereID, MatiereCoefficient FROM Matiere")}

        # Load all notes from session DB
        notes_by_candidat: dict = {}
        for r in self._conn.execute("SELECT CandidatID, MatiereID, Note FROM Note"):
            cid, mid, val = r[0], r[1], r[2]
            try:
                v = float(str(val).replace(",", "."))
            except (ValueError, TypeError):
                v = 0.0
            notes_by_candidat.setdefault(cid, []).append((mid, v))

        # Compute weighted average per candidat
        for cid, notes in notes_by_candidat.items():
            sum_w = sum(v * coeffs.get(mid, 1.0) for mid, v in notes)
            sum_c = sum(coeffs.get(mid, 1.0) for mid, _ in notes)
            moyenne = sum_w / sum_c if sum_c > 0 else 0.0
            self._conn.execute(
                "UPDATE Candidat SET CandidatMoyenne=? WHERE CandidatID=?",
                (round(moyenne, 4), cid)
            )

        # Rank all by moyenne DESC
        ranked = self._conn.execute(
            "SELECT CandidatID FROM Candidat ORDER BY CandidatMoyenne DESC NULLS LAST"
        ).fetchall()
        for i, row in enumerate(ranked):
            self._conn.execute(
                "UPDATE Candidat SET CandidatClassement=? WHERE CandidatID=?",
                (i + 1, row[0])
            )
        self._conn.commit()

    @staticmethod
    def _row_to_model(r) -> Candidat:
        return Candidat(
            candidat_id=r["CandidatID"],
            nom=r["Nom"] or "",
            nom_intermediaire=r["NomIntermediaire"] or "",
            prenom=r["Prenom"] or "",
            date_de_naissance=r["DateDeNaissance"],
            sexe=r["Sexe"] or "",
            candidat_statut=r["CandidatStatut"] or "I",
            langue=r["Langue"] or "fr",
            etab_id=r["EtabID"] or 0,
            candidat_moyenne=r["CandidatMoyenne"],
            candidat_classement=r["CandidatClassement"],
            anonymat=r["anonymat"] or ""
        )
