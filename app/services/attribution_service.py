"""
Port of VB6 moMain.bas AttributionSpecialite() / C# AttributionService.Run().

Algorithm:
1. Iterate candidates in ranked order (best average first).
2. For each candidate, try their ordered filiere choices.
3. Admit to the first filiere that still has capacity (count_admis < NbPlace).
4. If no filiere has capacity: candidate remains unassigned.

Call ChoixRepository.reset_admis() and
CandidatRepository.compute_averages_and_ranking() before run().
"""
from app.database.session_db import SessionDB
from app.database.repositories.candidat_repo import CandidatRepository
from app.database.repositories.choix_repo import ChoixRepository


class AttributionService:
    @staticmethod
    def run():
        conn = SessionDB.conn()
        candidat_repo = CandidatRepository()
        choix_repo = ChoixRepository()

        # Get candidates ranked by moyenne (best first), eligible for attribution
        candidate_ids = candidat_repo.get_ranked_for_attribution()

        for candidat_id in candidate_ids:
            # Load ordered choices with NbPlace from Filiere table
            choices = conn.execute(
                """SELECT ch.ChoixID, ch.FiliereID, f.NbPlace
                   FROM Choix ch
                   JOIN Filiere f ON ch.FiliereID = f.FiliereID
                   WHERE ch.CandidatID = ? AND ch.ChoixAdmis = 0
                   ORDER BY ch.ChoixOrdre""",
                (candidat_id,)
            ).fetchall()

            for choice in choices:
                choix_id = choice["ChoixID"]
                filiere_id = choice["FiliereID"]
                nb_place = choice["NbPlace"] or 0

                admis_count = choix_repo.count_admis(filiere_id)
                if admis_count < nb_place:
                    # Admit this candidate to this filiere
                    conn.execute(
                        "UPDATE Choix SET ChoixAdmis=1 WHERE ChoixID=?", (choix_id,)
                    )
                    conn.commit()
                    break  # Candidate assigned — move to next
            # If no filiere had capacity: candidate stays unassigned
