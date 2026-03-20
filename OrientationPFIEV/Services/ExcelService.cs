using System.Data;
using ClosedXML.Excel;

namespace OrientationPFIEV.Services;

/// <summary>
/// Excel import/export via ClosedXML. Replaces VB6 COM Interop + .xlt templates.
/// </summary>
public static class ExcelService
{
    // ── EXPORT ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Exports candidate list DataTable to Excel with styled header row.
    /// Columns: Classement, Nom, NomIntermediaire, Prenom, Statut, Moyenne, Filière, Etablissement
    /// </summary>
    public static void ExportCandidats(DataTable dt, string filePath)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Candidats");

        // Header row
        var headers = new[]
        {
            "Classement", "Nom", "Nom Intermédiaire", "Prénom",
            "Statut", "Moyenne", "Filière", "Etablissement"
        };
        // Map from DataTable column names to export columns (best-effort by name)
        var colMap = new[]
        {
            "CandidatClassement", "Nom", "NomIntermediaire", "Prenom",
            "CandidatStatut", "CandidatMoyenne", "FiliereCode", "EtabNom"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
        }

        // Data rows — map columns by name if available, otherwise by position
        for (int row = 0; row < dt.Rows.Count; row++)
        {
            for (int col = 0; col < colMap.Length; col++)
            {
                string colName = colMap[col];
                object? val = dt.Columns.Contains(colName)
                    ? dt.Rows[row][colName]
                    : (col < dt.Columns.Count ? dt.Rows[row][col] : null);
                ws.Cell(row + 2, col + 1).Value = val?.ToString() ?? "";
            }
        }

        ws.Columns().AdjustToContents();
        wb.SaveAs(filePath);
    }

    /// <summary>
    /// Exports results DataTable to Excel with title row and styled header.
    /// Row 1: title, Row 2: blank, Row 3: bold+yellow headers, Row 4+: data.
    /// </summary>
    public static void ExportResultats(DataTable dt, string filePath)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Résultats");

        // Title row
        var titleCell = ws.Cell(1, 1);
        titleCell.Value = "Résultats du concours PFIEV";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.FontSize = 14;

        // Row 2 is blank spacer — leave empty

        // Header row at row 3
        var headers = new[]
        {
            "Classement", "Nom", "Nom Intermédiaire", "Prénom",
            "Statut", "Moyenne", "Filière", "Etablissement"
        };
        var colMap = new[]
        {
            "CandidatClassement", "Nom", "NomIntermediaire", "Prenom",
            "CandidatStatut", "CandidatMoyenne", "FiliereCode", "EtabFiliere"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(3, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightYellow;
        }

        // Data rows starting at row 4
        for (int row = 0; row < dt.Rows.Count; row++)
        {
            for (int col = 0; col < colMap.Length; col++)
            {
                string colName = colMap[col];
                object? val = dt.Columns.Contains(colName)
                    ? dt.Rows[row][colName]
                    : (col < dt.Columns.Count ? dt.Rows[row][col] : null);
                ws.Cell(row + 4, col + 1).Value = val?.ToString() ?? "";
            }
        }

        ws.Columns().AdjustToContents();
        wb.SaveAs(filePath);
    }

    // ── IMPORT ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Reads candidates from Excel worksheet 1. Row 1 is header (skipped).
    /// Columns: A=ignored, B=Nom, C=NomIntermediaire, D=Prenom, E=DateDeNaissance,
    ///          F=Sexe, G=Statut, H=Langue, I=EtabCode, J+=Notes.
    /// </summary>
    public static List<ImportCandidatRow> ImportCandidats(string filePath)
    {
        var result = new List<ImportCandidatRow>();
        using var wb = new XLWorkbook(filePath);
        var ws = wb.Worksheet(1);
        int lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
        int lastCol = ws.LastColumnUsed()?.ColumnNumber() ?? 9;

        for (int row = 2; row <= lastRow; row++)
        {
            var r = new ImportCandidatRow
            {
                Nom              = ws.Cell(row, 2).GetString(),
                NomIntermediaire = ws.Cell(row, 3).GetString(),
                Prenom           = ws.Cell(row, 4).GetString(),
                DateDeNaissance  = ws.Cell(row, 5).GetString(),
                Sexe             = ws.Cell(row, 6).GetString(),
                Statut           = ws.Cell(row, 7).GetString(),
                Langue           = ws.Cell(row, 8).GetString(),
                EtabCode         = ws.Cell(row, 9).GetString(),
                Notes            = new List<string>()
            };

            // Columns J onwards (col 10+) are note values per matiere order
            for (int col = 10; col <= lastCol; col++)
                r.Notes.Add(ws.Cell(row, col).GetString());

            result.Add(r);
        }
        return result;
    }
}

/// <summary>One row read from the import Excel template.</summary>
public class ImportCandidatRow
{
    public string Nom              { get; set; } = "";
    public string NomIntermediaire { get; set; } = "";
    public string Prenom           { get; set; } = "";
    public string DateDeNaissance  { get; set; } = "";
    public string Sexe             { get; set; } = "";
    public string Statut           { get; set; } = "";
    public string Langue           { get; set; } = "";
    public string EtabCode         { get; set; } = "";
    public List<string> Notes      { get; set; } = new();
}
