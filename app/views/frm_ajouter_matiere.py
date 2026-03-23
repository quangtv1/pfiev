from typing import Optional
from PySide6.QtWidgets import (
    QDialog, QFormLayout, QLineEdit, QDialogButtonBox, QMessageBox
)
from app.localization import L
from app.models.matiere import Matiere


class FrmAjouterMatiere(QDialog):
    def __init__(self, parent=None, matiere: Optional[Matiere] = None):
        super().__init__(parent)
        self._matiere = matiere
        title = L.get("TitleModifierMatiere") if matiere else L.get("TitleAjouterMatiere")
        self.setWindowTitle(title)
        self.setFixedSize(300, 160)
        self._setup_ui()
        if matiere:
            self._txt_nom.setText(matiere.matiere_nom)
            self._txt_code.setText(matiere.matiere_code)
            self._txt_coeff.setText(str(matiere.matiere_coefficient))

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._txt_nom = QLineEdit()
        self._txt_code = QLineEdit()
        self._txt_coeff = QLineEdit("1.0")
        layout.addRow(L.get("LblNom"), self._txt_nom)
        layout.addRow(L.get("LblCode"), self._txt_code)
        layout.addRow(L.get("LblCoefficient"), self._txt_coeff)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _on_ok(self):
        nom = self._txt_nom.text().strip()
        if not nom:
            QMessageBox.warning(self, "", L.get("MsgFieldRequired"))
            return
        try:
            coeff = float(self._txt_coeff.text().replace(",", ".") or "1")
        except ValueError:
            QMessageBox.warning(self, "", L.get("MsgOnlyNumbers"))
            return

        from app.database.repositories.matiere_repo import MatiereRepository
        m = Matiere(
            matiere_id=self._matiere.matiere_id if self._matiere else 0,
            matiere_nom=nom,
            matiere_coefficient=coeff,
            matiere_code=self._txt_code.text().strip()
        )
        repo = MatiereRepository()
        if self._matiere:
            repo.update(m)
        else:
            repo.add(m)
        self.accept()
