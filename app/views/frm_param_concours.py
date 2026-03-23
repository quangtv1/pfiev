from PySide6.QtWidgets import (
    QDialog, QFormLayout, QSpinBox, QDoubleSpinBox, QDialogButtonBox
)
from app.localization import L
from app.models.concours import Concours


class FrmParamConcours(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleParamConcours"))
        self.setFixedSize(280, 130)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._spin_annee = QSpinBox()
        self._spin_annee.setRange(2000, 2100)
        self._spin_moy = QDoubleSpinBox()
        self._spin_moy.setRange(0.0, 20.0)
        self._spin_moy.setDecimals(2)
        layout.addRow(L.get("LblAnnee"), self._spin_annee)
        layout.addRow(L.get("LblMoyMin"), self._spin_moy)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _load(self):
        from app.database.repositories.concours_repo import ConcoursRepository
        c = ConcoursRepository().get_params()
        if c:
            self._spin_annee.setValue(c.annee)
            self._spin_moy.setValue(c.moyenne_min)

    def _on_ok(self):
        from app.database.repositories.concours_repo import ConcoursRepository
        ConcoursRepository().set_params(Concours(
            annee=self._spin_annee.value(),
            moyenne_min=self._spin_moy.value()
        ))
        self.accept()
