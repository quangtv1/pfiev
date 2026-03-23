namespace OrientationPFIEV.Models;

/// <summary>
/// Grade entry for one candidate in one subject.
/// NoteValue is stored as string to handle locale-dependent decimals (e.g. "12,5" vs "12.5").
/// </summary>
public class Note
{
    public int NoteID { get; set; }
    public int CandidatID { get; set; }
    public int MatiereID { get; set; }
    public string NoteValue { get; set; } = "";
}
