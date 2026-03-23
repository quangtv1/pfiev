namespace OrientationPFIEV.Models;

/// <summary>
/// Exam candidate. CandidatStatut: "I" = internal, "E" = external.
/// Langue: "fr" or "vi". Sexe: "M" or "F".
/// </summary>
public class Candidat
{
    public int CandidatID { get; set; }
    public string Nom { get; set; } = "";
    public string NomIntermediaire { get; set; } = "";
    public string Prenom { get; set; } = "";
    public DateTime? DateDeNaissance { get; set; }
    public string Sexe { get; set; } = "";
    public string CandidatStatut { get; set; } = "";
    public string Langue { get; set; } = "";
    public int EtabID { get; set; }
    public double? CandidatMoyenne { get; set; }
    public int? CandidatClassement { get; set; }
    public string Anonymat { get; set; } = "";
}
