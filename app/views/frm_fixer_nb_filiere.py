from PySide6.QtWidgets import (
    QDialog, QFormLayout, QSpinBox, QDialogButtonBox, QLabel
)
from app.localization import L
from app.state import AppState


class FrmFixerNbFiliere(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleFixerNbFiliere"))
        self.setFixedSize(260, 120)
        self._setup_ui()

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._spin = QSpinBox()
        self._spin.setRange(1, 20)
        self._spin.setValue(AppState.max_choices)
        layout.addRow(L.get("LblMaxChoices"), self._spin)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _on_ok(self):
        AppState.max_choices = self._spin.value()
        self.accept()
