using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;

namespace OrientationPFIEV.Services;

/// <summary>
/// Port of VB6 moMain.bas AttributionSpecialite().
/// Iterates candidates in ranked order; for each candidate tries their ordered
/// filiere choices and admits them to the first filiere that still has capacity.
/// Call ChoixRepository.ResetAdmis() and CandidatRepository.ComputeAveragesAndRanking()
/// before invoking Run().
/// </summary>
public static class AttributionService
{
    public static void Run(DatabaseContext sessionDb)
    {
        var candidatRepo = new CandidatRepository(sessionDb);
        var choixRepo    = new ChoixRepository(sessionDb);

        // Ranked candidates: internal (I) all + external (E) above MoyenneMin
        var candidateIds = candidatRepo.GetRankedForAttribution();

        foreach (var candidatId in candidateIds)
        {
            var choices = choixRepo.GetOrderedChoices(candidatId);

            foreach (var choix in choices)
            {
                int admisCount = choixRepo.CountAdmis(choix.FiliereID);
                if (admisCount < choix.FiliereNbPlace)
                {
                    choixRepo.SetAdmis(choix.ChoixID, true);
                    break; // candidate is assigned — move to next
                }
            }
            // If no filiere had capacity: candidate remains unassigned
        }
    }
}
