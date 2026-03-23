from PySide6.QtWidgets import QDialog, QVBoxLayout, QTableWidget, QTableWidgetItem, QDialogButtonBox
from app.localization import L


class FrmStatEtab(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleStatEtab"))
        self.resize(500, 350)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(4)
        self._table.setHorizontalHeaderLabels(
            [L.get("LblSchool"), L.get("ColNbTotal"), L.get("ColNbInternes"), L.get("ColNbExternes")]
        )
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)
        btns = QDialogButtonBox(QDialogButtonBox.Close)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.resultats_repo import ResultatsRepository
        rows = ResultatsRepository().get_stat_etab()
        self._table.setRowCount(len(rows))
        for r, row in enumerate(rows):
            self._table.setItem(r, 0, QTableWidgetItem(row.get("EtabNom") or ""))
            self._table.setItem(r, 1, QTableWidgetItem(str(row.get("NbCandidats") or 0)))
            self._table.setItem(r, 2, QTableWidgetItem(str(row.get("NbInternes") or 0)))
            self._table.setItem(r, 3, QTableWidgetItem(str(row.get("NbExternes") or 0)))
        self._table.resizeColumnsToContents()
