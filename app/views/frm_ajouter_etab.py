from typing import Optional
from PySide6.QtWidgets import (
    QDialog, QFormLayout, QLineEdit, QDialogButtonBox, QMessageBox
)
from app.localization import L
from app.models.etablissement import Etablissement


class FrmAjouterEtab(QDialog):
    def __init__(self, parent=None, etab: Optional[Etablissement] = None):
        super().__init__(parent)
        self._etab = etab
        title = L.get("TitleModifierEtab") if etab else L.get("TitleAjouterEtab")
        self.setWindowTitle(title)
        self.setFixedSize(300, 130)
        self._setup_ui()
        if etab:
            self._txt_nom.setText(etab.etab_nom)
            self._txt_code.setText(etab.etab_code)

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._txt_nom = QLineEdit()
        self._txt_code = QLineEdit()
        layout.addRow(L.get("LblNom"), self._txt_nom)
        layout.addRow(L.get("LblCode"), self._txt_code)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _on_ok(self):
        nom = self._txt_nom.text().strip()
        if not nom:
            QMessageBox.warning(self, "", L.get("MsgFieldRequired"))
            return
        from app.database.repositories.etablissement_repo import EtablissementRepository
        e = Etablissement(
            etab_id=self._etab.etab_id if self._etab else 0,
            etab_nom=nom,
            etab_code=self._txt_code.text().strip()
        )
        repo = EtablissementRepository()
        if self._etab:
            repo.update(e)
        else:
            repo.add(e)
        self.accept()
