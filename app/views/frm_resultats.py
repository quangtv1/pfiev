from PySide6.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QTableWidget, QTableWidgetItem,
    QPushButton, QMessageBox, QDialog
)
from app.localization import L


class FrmResultats(QWidget):
    COLUMNS = ["CandidatClassement", "Nom", "Prenom", "CandidatStatut",
               "CandidatMoyenne", "FiliereNom", "EtabFiliere"]

    def __init__(self, read_only: bool = False):
        super().__init__()
        self._read_only = read_only
        self._tableau = None
        self.setWindowTitle(L.get("TitleResultats"))
        self.resize(900, 600)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._table = QTableWidget()
        headers = [
            L.get("ColClassement"), L.get("ColNom"), L.get("ColPrenom"),
            L.get("ColStatut"), L.get("ColMoyenne"),
            L.get("ColFiliereAdmis"), L.get("ColEtabFiliere")
        ]
        self._table.setColumnCount(len(headers))
        self._table.setHorizontalHeaderLabels(headers)
        self._table.setSelectionBehavior(QTableWidget.SelectRows)
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)

        btn_layout = QHBoxLayout()
        if not self._read_only:
            btn_export = QPushButton(L.get("BtnExport"))
            btn_tableaux = QPushButton(L.get("BtnTableaux"))
            btn_changer = QPushButton(L.get("TitleChangerSpe"))
            btn_export.clicked.connect(self._on_export)
            btn_tableaux.clicked.connect(self._on_tableaux)
            btn_changer.clicked.connect(self._on_changer)
            btn_layout.addWidget(btn_export)
            btn_layout.addWidget(btn_tableaux)
            btn_layout.addWidget(btn_changer)
        btn_retour = QPushButton(L.get("BtnRetour"))
        btn_retour.clicked.connect(self.close)
        btn_layout.addStretch()
        btn_layout.addWidget(btn_retour)
        layout.addLayout(btn_layout)

    def _load(self):
        from app.database.repositories.resultats_repo import ResultatsRepository
        rows = ResultatsRepository().get_resultats()
        self._table.setRowCount(len(rows))
        for r, row in enumerate(rows):
            for c, col in enumerate(self.COLUMNS):
                val = str(row.get(col) or "")
                self._table.setItem(r, c, QTableWidgetItem(val))
        self._table.resizeColumnsToContents()

    def _on_export(self):
        from app.views.frm_export_excel import FrmExportExcel
        FrmExportExcel(self).exec()

    def _on_tableaux(self):
        from app.views.frm_tableau_recap import FrmTableauRecap
        self._tableau = FrmTableauRecap()
        self._tableau.show()

    def _on_changer(self):
        row = self._table.currentRow()
        if row < 0:
            return
        # Get CandidatID from hidden column (use resultats data)
        from app.database.repositories.resultats_repo import ResultatsRepository
        rows = ResultatsRepository().get_resultats()
        if row < len(rows):
            cid = rows[row].get("CandidatID")
            if cid:
                from app.views.frm_changer_spe import FrmChangerSpe
                if FrmChangerSpe(cid, self).exec() == QDialog.Accepted:
                    self._load()
