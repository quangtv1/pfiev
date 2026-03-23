from PySide6.QtWidgets import QDialog, QVBoxLayout, QTableWidget, QTableWidgetItem, QDialogButtonBox
from app.localization import L


class FrmStat(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleStat"))
        self.resize(500, 350)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(4)
        self._table.setHorizontalHeaderLabels(
            [L.get("ColFiliere"), L.get("ColNbAdmis"), L.get("ColNbTotal"), L.get("ColMoyenneSpe")]
        )
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)
        btns = QDialogButtonBox(QDialogButtonBox.Close)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.resultats_repo import ResultatsRepository
        rows = ResultatsRepository().get_classement_moyenne_spe()
        self._table.setRowCount(len(rows))
        for r, row in enumerate(rows):
            self._table.setItem(r, 0, QTableWidgetItem(row.get("FiliereNom") or ""))
            self._table.setItem(r, 1, QTableWidgetItem(str(row.get("NbAdmis") or 0)))
            self._table.setItem(r, 2, QTableWidgetItem(str(row.get("NbAdmis") or 0)))
            moy = row.get("MoyenneSpe")
            self._table.setItem(r, 3, QTableWidgetItem(f"{moy:.2f}" if moy else ""))
        self._table.resizeColumnsToContents()
