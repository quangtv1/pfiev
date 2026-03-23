from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QTableWidget, QTableWidgetItem,
    QDialogButtonBox, QHBoxLayout, QLabel, QSpinBox, QPushButton, QMessageBox
)
from PySide6.QtCore import Qt
from app.localization import L


class FrmFixerNbrePlace(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleFixerNbrePlace"))
        self.setFixedSize(400, 350)
        self._filieres = []
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)

        # Global setter
        top = QHBoxLayout()
        top.addWidget(QLabel(L.get("LblNbPlaceGlobal") + ":"))
        self._spin_global = QSpinBox()
        self._spin_global.setRange(0, 9999)
        top.addWidget(self._spin_global)
        btn_apply = QPushButton(L.get("BtnApplyAll"))
        btn_apply.clicked.connect(self._apply_global)
        top.addWidget(btn_apply)
        layout.addLayout(top)

        # Per-filiere table
        self._table = QTableWidget()
        self._table.setColumnCount(2)
        self._table.setHorizontalHeaderLabels([L.get("ColFiliere"), L.get("LblNbPlace")])
        self._table.horizontalHeader().setStretchLastSection(True)
        self._table.setSelectionBehavior(QTableWidget.SelectRows)
        layout.addWidget(self._table)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.filiere_repo import FiliereRepository
        self._filieres = FiliereRepository().get_all()
        self._table.setRowCount(len(self._filieres))
        for r, f in enumerate(self._filieres):
            self._table.setItem(r, 0, QTableWidgetItem(f.filiere_nom))
            self._table.setItem(r, 0, QTableWidgetItem(f.filiere_nom))
            spin = QSpinBox()
            spin.setRange(0, 9999)
            spin.setValue(f.nb_place)
            self._table.setCellWidget(r, 1, spin)
        self._table.resizeColumnsToContents()

    def _apply_global(self):
        val = self._spin_global.value()
        for r in range(self._table.rowCount()):
            spin = self._table.cellWidget(r, 1)
            if spin:
                spin.setValue(val)

    def _on_ok(self):
        from app.database.repositories.filiere_repo import FiliereRepository
        repo = FiliereRepository()
        for r, f in enumerate(self._filieres):
            spin = self._table.cellWidget(r, 1)
            if spin:
                repo.update_nb_place(f.filiere_id, spin.value())
        self.accept()
