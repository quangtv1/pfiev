from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QRadioButton, QDialogButtonBox,
    QFileDialog, QMessageBox
)
from app.localization import L


class FrmExportExcel(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleExportExcel"))
        self.setFixedSize(320, 130)
        self._setup_ui()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._radio_candidats = QRadioButton(L.get("OptExportCandidats"))
        self._radio_resultats = QRadioButton(L.get("OptExportResultats"))
        self._radio_candidats.setChecked(True)
        layout.addWidget(self._radio_candidats)
        layout.addWidget(self._radio_resultats)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_export)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _on_export(self):
        from datetime import date
        today = date.today().strftime("%Y%m%d")
        if self._radio_candidats.isChecked():
            default_name = f"danh_sach_thi_sinh_{today}.xlsx"
        else:
            default_name = f"ket_qua_phan_bo_{today}.xlsx"
        path, _ = QFileDialog.getSaveFileName(
            self, "", default_name, "Excel (*.xlsx)"
        )
        if not path:
            return
        if not path.lower().endswith(".xlsx"):
            path += ".xlsx"
        try:
            from app.services.excel_service import ExcelService
            if self._radio_candidats.isChecked():
                from app.database.repositories.candidat_repo import CandidatRepository
                rows = CandidatRepository().get_session_view()
                ExcelService.export_candidats(rows, path)
            else:
                from app.database.repositories.resultats_repo import ResultatsRepository
                rows = ResultatsRepository().get_resultats()
                ExcelService.export_resultats(rows, path)
            QMessageBox.information(self, "", L.get("MsgExportDone"))
            self.accept()
        except Exception as e:
            QMessageBox.critical(self, "", str(e))
