namespace OrientationPFIEV.Models;

/// <summary>Subject/exam paper with its weighting coefficient.</summary>
public class Matiere
{
    public int MatiereID { get; set; }
    public string MatiereNom { get; set; } = "";
    public double MatiereCoefficient { get; set; }
}
