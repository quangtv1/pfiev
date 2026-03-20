namespace OrientationPFIEV.Models;

/// <summary>
/// A candidate's program preference choice.
/// NumeroDeChoix: 1 = first choice, 2 = second, etc.
/// ChoixAdmis: true when the candidate is admitted through this choice.
/// </summary>
public class Choix
{
    public int ChoixID { get; set; }
    public int CandidatID { get; set; }
    public int FiliereID { get; set; }
    public int NumeroDeChoix { get; set; }
    public bool ChoixAdmis { get; set; }
}
