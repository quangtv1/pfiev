namespace OrientationPFIEV.Models;

/// <summary>School/institution record from CONFIG.MDB.</summary>
public class Etablissement
{
    public int EtabID { get; set; }
    public string EtabNom { get; set; } = "";
    public string EtabCode { get; set; } = "";
    public string EtabVille { get; set; } = "";
}
