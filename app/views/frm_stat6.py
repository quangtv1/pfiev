from PySide6.QtWidgets import QDialog, QVBoxLayout, QTableWidget, QTableWidgetItem, QDialogButtonBox
from app.localization import L


class FrmStat6(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleStat6"))
        self.resize(600, 400)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(5)
        self._table.setHorizontalHeaderLabels(
            [L.get("ColFiliere"), L.get("ColNom"), L.get("ColPrenom"),
             L.get("ColMoyenne"), L.get("ColClassement")]
        )
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)
        btns = QDialogButtonBox(QDialogButtonBox.Close)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.resultats_repo import ResultatsRepository
        rows = ResultatsRepository().get_stat_top_per_filiere()
        self._table.setRowCount(len(rows))
        for r, row in enumerate(rows):
            self._table.setItem(r, 0, QTableWidgetItem(row.get("FiliereNom") or ""))
            self._table.setItem(r, 1, QTableWidgetItem(row.get("Nom") or ""))
            self._table.setItem(r, 2, QTableWidgetItem(row.get("Prenom") or ""))
            moy = row.get("CandidatMoyenne")
            self._table.setItem(r, 3, QTableWidgetItem(f"{moy:.2f}" if moy else ""))
            self._table.setItem(r, 4, QTableWidgetItem(str(row.get("CandidatClassement") or "")))
        self._table.resizeColumnsToContents()
