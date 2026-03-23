from PySide6.QtWidgets import QWidget, QVBoxLayout, QPushButton, QHBoxLayout
from app.localization import L


class FrmResultatsGraphe(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle(L.get("TitleGraphe"))
        self.resize(900, 500)
        self._setup_ui()
        self._setup_charts()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._chart_area = QVBoxLayout()
        layout.addLayout(self._chart_area)

        btn_row = QHBoxLayout()
        btn_close = QPushButton(L.get("BtnClose"))
        btn_close.clicked.connect(self.close)
        btn_row.addStretch()
        btn_row.addWidget(btn_close)
        layout.addLayout(btn_row)

    def _setup_charts(self):
        try:
            from matplotlib.backends.backend_qtagg import FigureCanvasQTAgg
            from matplotlib.figure import Figure
            from app.database.repositories.resultats_repo import ResultatsRepository

            repo = ResultatsRepository()
            fig = Figure(figsize=(12, 5))

            # Bar chart: NbAdmis per filiere
            ax1 = fig.add_subplot(121)
            data = repo.get_classement_moyenne_spe()
            if data:
                filieres = [r["FiliereNom"] or "" for r in data]
                nb_admis = [r["NbAdmis"] or 0 for r in data]
                ax1.bar(filieres, nb_admis, color="steelblue")
                ax1.set_title(L.get("ChartTitleAdmisFiliere"))
                ax1.tick_params(axis="x", rotation=45)
                ax1.set_ylabel(L.get("ChartYNbAdmis"))
            else:
                ax1.text(0.5, 0.5, L.get("MsgNoData"), ha="center", va="center",
                         transform=ax1.transAxes)
                ax1.set_title(L.get("ChartTitleAdmisFiliere"))

            # Pie chart: Internes vs Externes
            ax2 = fig.add_subplot(122)
            stat = repo.get_stat_ie()
            nb_int = stat.get("NbInternes", 0)
            nb_ext = stat.get("NbExternes", 0)
            if nb_int + nb_ext > 0:
                ax2.pie(
                    [nb_int, nb_ext],
                    labels=[L.get("LblInternes"), L.get("LblExternes")],
                    autopct="%1.1f%%",
                    colors=["#4C72B0", "#DD8452"]
                )
            else:
                ax2.text(0.5, 0.5, L.get("MsgNoData"), ha="center", va="center",
                         transform=ax2.transAxes)
            ax2.set_title(L.get("ChartTitleRepartitionIE"))

            canvas = FigureCanvasQTAgg(fig)
            fig.tight_layout()
            self._chart_area.addWidget(canvas)

        except Exception as e:
            from PySide6.QtWidgets import QLabel
            self._chart_area.addWidget(QLabel(f"{L.get('MsgChartError')}: {e}"))
