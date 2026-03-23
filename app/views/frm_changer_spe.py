from typing import Optional
from PySide6.QtWidgets import (
    QDialog, QFormLayout, QLabel, QComboBox, QDialogButtonBox, QMessageBox
)
from app.localization import L


class FrmChangerSpe(QDialog):
    def __init__(self, candidat_id: int, parent=None):
        super().__init__(parent)
        self._candidat_id = candidat_id
        self._filieres = []
        self._old_filiere_id: Optional[int] = None
        self.setWindowTitle(L.get("TitleChangerSpe"))
        self.setFixedSize(340, 150)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._lbl_current = QLabel()
        self._combo_new = QComboBox()
        layout.addRow(L.get("LblFiliereActuelle"), self._lbl_current)
        layout.addRow(L.get("LblNouvelleFiliere"), self._combo_new)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _load(self):
        from app.database.repositories.filiere_repo import FiliereRepository
        from app.database.repositories.choix_repo import ChoixRepository
        self._filieres = FiliereRepository(use_session_db=True).get_all()
        self._old_filiere_id = ChoixRepository().get_admitted_filiere_id(self._candidat_id)

        current_name = ""
        for f in self._filieres:
            if f.filiere_id == self._old_filiere_id:
                current_name = f.filiere_nom
                break
        self._lbl_current.setText(current_name or "—")

        self._combo_new.clear()
        for f in self._filieres:
            self._combo_new.addItem(f.filiere_nom)
        # Pre-select the current filiere
        if current_name:
            idx = self._combo_new.findText(current_name)
            if idx >= 0:
                self._combo_new.setCurrentIndex(idx)

    def _on_ok(self):
        if not self._filieres or self._combo_new.currentIndex() < 0:
            return
        new_filiere = self._filieres[self._combo_new.currentIndex()]
        if self._old_filiere_id and new_filiere.filiere_id != self._old_filiere_id:
            from app.database.repositories.choix_repo import ChoixRepository
            ChoixRepository().change_spe(
                self._candidat_id, self._old_filiere_id, new_filiere.filiere_id
            )
        self.accept()
