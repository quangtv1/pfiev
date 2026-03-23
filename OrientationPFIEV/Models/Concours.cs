namespace OrientationPFIEV.Models;

/// <summary>Competition session parameters: year, minimum average, and line count.</summary>
public class Concours
{
    public int Annee { get; set; }
    public double MoyenneMin { get; set; }
    public int NumLigne { get; set; }
}
