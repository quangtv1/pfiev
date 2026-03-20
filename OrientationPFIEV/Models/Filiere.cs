namespace OrientationPFIEV.Models;

/// <summary>Academic program/track belonging to an Etablissement.</summary>
public class Filiere
{
    public int FiliereID { get; set; }
    public string FiliereNom { get; set; } = "";
    public string FiliereCode { get; set; } = "";
    public int FiliereNbPlace { get; set; }
    public int EtabID { get; set; }
}
