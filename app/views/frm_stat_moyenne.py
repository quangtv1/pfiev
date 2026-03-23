from PySide6.QtWidgets import QDialog, QVBoxLayout, QTableWidget, QTableWidgetItem, QDialogButtonBox
from app.localization import L


class FrmStatMoyenne(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleStatMoyenne"))
        self.resize(350, 250)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(2)
        self._table.setHorizontalHeaderLabels([L.get("ColTranche"), L.get("ColNbCandidats")])
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)
        btns = QDialogButtonBox(QDialogButtonBox.Close)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.resultats_repo import ResultatsRepository
        rows = ResultatsRepository().get_score_distribution()
        self._table.setRowCount(len(rows))
        for r, row in enumerate(rows):
            self._table.setItem(r, 0, QTableWidgetItem(row.get("Range") or ""))
            self._table.setItem(r, 1, QTableWidgetItem(str(row.get("NbCandidats") or 0)))
        self._table.resizeColumnsToContents()
