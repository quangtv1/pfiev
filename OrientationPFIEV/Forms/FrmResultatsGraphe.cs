using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OrientationPFIEV.Data;
using OrientationPFIEV.Data.Repositories;
using OrientationPFIEV.Helpers;
using System.Data;

namespace OrientationPFIEV.Forms;

/// <summary>
/// Charts form: Bar chart (NbAdmis per filiere) + Pie chart (Internes vs Externes).
/// Uses OxyPlot.WindowsForms PlotView controls created programmatically on load
/// because PlotView is not a standard WinForms designer toolbox component.
/// </summary>
public partial class FrmResultatsGraphe : Form
{
    private readonly ResultatsRepository _repo;
    private PlotView _barView  = null!;
    private PlotView _pieView  = null!;

    public FrmResultatsGraphe()
    {
        InitializeComponent();
        _repo = new ResultatsRepository(SessionDatabase.Context);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyLocalization();
        CreatePlotViews();
        LoadBarChart();
        LoadPieChart();
    }

    private void ApplyLocalization()
    {
        Text = L.Get("TitleGraphe");
        btnClose.Text = L.Get("BtnClose");
    }

    // ── OxyPlot controls (created programmatically — not in designer) ────────

    private void CreatePlotViews()
    {
        _barView = new PlotView
        {
            Location = new System.Drawing.Point(12, 12),
            Size     = new System.Drawing.Size(480, 420),
            TabIndex = 0
        };
        _pieView = new PlotView
        {
            Location = new System.Drawing.Point(504, 12),
            Size     = new System.Drawing.Size(480, 420),
            TabIndex = 1
        };
        Controls.Add(_barView);
        Controls.Add(_pieView);
    }

    // ── Bar chart: NbAdmis per Filiere ───────────────────────────────────────

    private void LoadBarChart()
    {
        try
        {
            var dt = _repo.GetClassementMoyenneSpe();
            var model = new PlotModel { Title = "Admis par filière" };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title    = "Filière"
            };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title    = "Nombre admis",
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            };
            var series = new BarSeries
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}"
            };

            foreach (DataRow row in dt.Rows)
            {
                categoryAxis.Labels.Add(row["FiliereNom"]?.ToString() ?? "");
                series.Items.Add(new BarItem(Convert.ToDouble(row["NbAdmis"])));
            }

            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            model.Series.Add(series);
            _barView.Model = model;
        }
        catch (Exception ex)
        {
            // Non-fatal: show error in plot title so the form still opens
            _barView.Model = new PlotModel { Title = $"Erreur: {ex.Message}" };
        }
    }

    // ── Pie chart: Internes vs Externes ──────────────────────────────────────

    private void LoadPieChart()
    {
        try
        {
            var dt = _repo.GetResultats();
            int nbInternes = 0, nbExternes = 0;
            foreach (DataRow row in dt.Rows)
            {
                var statut = row["CandidatStatut"]?.ToString() ?? "";
                if (statut == "I") nbInternes++;
                else if (statut == "E") nbExternes++;
            }

            var model = new PlotModel { Title = "Répartition Internes / Externes" };
            var series = new PieSeries
            {
                StrokeThickness     = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan           = 360,
                StartAngle          = 0
            };
            series.Slices.Add(new PieSlice("Internes", nbInternes) { IsExploded = false });
            series.Slices.Add(new PieSlice("Externes", nbExternes) { IsExploded = false });
            model.Series.Add(series);
            _pieView.Model = model;
        }
        catch (Exception ex)
        {
            _pieView.Model = new PlotModel { Title = $"Erreur: {ex.Message}" };
        }
    }

    private void btnClose_Click(object s, EventArgs e) => Close();
}
