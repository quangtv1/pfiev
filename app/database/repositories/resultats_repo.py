from app.database.session_db import SessionDB


class ResultatsRepository:
    def __init__(self):
        self._conn = SessionDB.conn()

    def get_resultats(self) -> list:
        """Full results: candidat + assigned filiere + etab."""
        sql = """
            SELECT c.CandidatID, c.Nom, c.NomIntermediaire, c.Prenom,
                   c.CandidatStatut, c.CandidatMoyenne, c.CandidatClassement,
                   f.FiliereCode, f.FiliereNom, e.EtabNom AS EtabFiliere
            FROM Candidat c
            LEFT JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = 1
            LEFT JOIN Filiere f ON ch.FiliereID = f.FiliereID
            LEFT JOIN Etablissement e ON f.EtabID = e.EtabID
            ORDER BY c.CandidatClassement
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_classement_moyenne_spe(self) -> list:
        """Average score and count per filiere (admitted only)."""
        sql = """
            SELECT f.FiliereNom, AVG(c.CandidatMoyenne) AS MoyenneSpe,
                   COUNT(c.CandidatID) AS NbAdmis
            FROM Filiere f
            INNER JOIN Choix ch ON f.FiliereID = ch.FiliereID AND ch.ChoixAdmis = 1
            INNER JOIN Candidat c ON ch.CandidatID = c.CandidatID
            GROUP BY f.FiliereNom
            ORDER BY f.FiliereNom
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_stat_etab(self) -> list:
        """Stats per school: total, internes, externes."""
        sql = """
            SELECT e.EtabNom,
                   COUNT(c.CandidatID) AS NbCandidats,
                   SUM(CASE WHEN c.CandidatStatut='I' THEN 1 ELSE 0 END) AS NbInternes,
                   SUM(CASE WHEN c.CandidatStatut='E' THEN 1 ELSE 0 END) AS NbExternes
            FROM Etablissement e
            INNER JOIN Candidat c ON e.EtabID = c.EtabID
            GROUP BY e.EtabNom
            ORDER BY e.EtabNom
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_stat_ie(self) -> dict:
        """Total internes vs externes counts."""
        row = self._conn.execute(
            """SELECT
                SUM(CASE WHEN CandidatStatut='I' THEN 1 ELSE 0 END) AS NbInternes,
                SUM(CASE WHEN CandidatStatut='E' THEN 1 ELSE 0 END) AS NbExternes
               FROM Candidat"""
        ).fetchone()
        return {"NbInternes": row[0] or 0, "NbExternes": row[1] or 0}

    def get_tableau_recap_internes(self) -> list:
        """Recap table: internes admitted per filiere."""
        sql = """
            SELECT f.FiliereNom, f.NbPlace,
                   COUNT(c.CandidatID) AS NbAdmis,
                   AVG(c.CandidatMoyenne) AS MoyenneSpe
            FROM Filiere f
            LEFT JOIN Choix ch ON f.FiliereID = ch.FiliereID AND ch.ChoixAdmis = 1
            LEFT JOIN Candidat c ON ch.CandidatID = c.CandidatID AND c.CandidatStatut='I'
            GROUP BY f.FiliereNom, f.NbPlace
            ORDER BY f.FiliereNom
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_tableau_recap_externes(self) -> list:
        """Recap table: externes admitted per filiere."""
        sql = """
            SELECT f.FiliereNom, f.NbPlace,
                   COUNT(c.CandidatID) AS NbAdmis,
                   AVG(c.CandidatMoyenne) AS MoyenneSpe
            FROM Filiere f
            LEFT JOIN Choix ch ON f.FiliereID = ch.FiliereID AND ch.ChoixAdmis = 1
            LEFT JOIN Candidat c ON ch.CandidatID = c.CandidatID AND c.CandidatStatut='E'
            GROUP BY f.FiliereNom, f.NbPlace
            ORDER BY f.FiliereNom
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_stat_by_language(self) -> list:
        """Stats by candidate language."""
        sql = """
            SELECT c.Langue,
                   COUNT(c.CandidatID) AS NbTotal,
                   SUM(CASE WHEN ch.ChoixAdmis=1 THEN 1 ELSE 0 END) AS NbAdmis
            FROM Candidat c
            LEFT JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = 1
            GROUP BY c.Langue
        """
        return [dict(r) for r in self._conn.execute(sql).fetchall()]

    def get_stat_top_per_filiere(self) -> list:
        """Top 6 candidates per filiere (sorted by moyenne DESC, grouped in Python)."""
        sql = """
            SELECT f.FiliereID, f.FiliereNom, c.Nom, c.Prenom,
                   c.CandidatMoyenne, c.CandidatClassement
            FROM Candidat c
            INNER JOIN Choix ch ON c.CandidatID = ch.CandidatID AND ch.ChoixAdmis = 1
            INNER JOIN Filiere f ON ch.FiliereID = f.FiliereID
            ORDER BY f.FiliereID, c.CandidatMoyenne DESC
        """
        rows = [dict(r) for r in self._conn.execute(sql).fetchall()]
        result = []
        counts: dict = {}
        for row in rows:
            fid = row.get("FiliereID")
            counts[fid] = counts.get(fid, 0) + 1
            if counts[fid] <= 6:
                row["RowNum"] = counts[fid]
                result.append(row)
        return result

    def get_score_distribution(self) -> list:
        """Score ranges for histogram: 0-4, 5-9, 10-14, 15-20."""
        buckets = [
            ("0-4", 0, 4),
            ("5-9", 5, 9),
            ("10-14", 10, 14),
            ("15-20", 15, 20),
        ]
        result = []
        for label, lo, hi in buckets:
            row = self._conn.execute(
                "SELECT COUNT(*) FROM Candidat WHERE CandidatMoyenne BETWEEN ? AND ?",
                (lo, hi)
            ).fetchone()
            result.append({"Range": label, "NbCandidats": row[0] if row else 0})
        return result
