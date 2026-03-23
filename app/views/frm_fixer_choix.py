from PySide6.QtWidgets import (
    QDialog, QHBoxLayout, QVBoxLayout, QListWidget, QPushButton,
    QLabel, QDialogButtonBox
)
from PySide6.QtCore import Qt
from app.localization import L
from app.state import AppState


class FrmFixerChoix(QDialog):
    def __init__(self, candidat_id: int, parent=None):
        super().__init__(parent)
        self._candidat_id = candidat_id
        self._filieres = []
        self.setWindowTitle(L.get("TitleFixerChoix"))
        self.setFixedSize(520, 380)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        cols = QHBoxLayout()

        # Available filieres
        left = QVBoxLayout()
        left.addWidget(QLabel(L.get("LblAvailableFilieres")))
        self._list_available = QListWidget()
        left.addWidget(self._list_available)
        cols.addLayout(left)

        # Center buttons
        mid = QVBoxLayout()
        mid.addStretch()
        btn_add = QPushButton(">")
        btn_remove = QPushButton("<")
        btn_up = QPushButton("↑")
        btn_down = QPushButton("↓")
        btn_add.clicked.connect(self._on_add)
        btn_remove.clicked.connect(self._on_remove)
        btn_up.clicked.connect(self._on_up)
        btn_down.clicked.connect(self._on_down)
        for b in [btn_add, btn_remove, btn_up, btn_down]:
            b.setFixedWidth(40)
            mid.addWidget(b)
        mid.addStretch()
        cols.addLayout(mid)

        # Selected choices in order
        right = QVBoxLayout()
        right.addWidget(QLabel(L.get("LblCandidateChoices")))
        self._list_choices = QListWidget()
        right.addWidget(self._list_choices)
        cols.addLayout(right)

        layout.addLayout(cols)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.filiere_repo import FiliereRepository
        from app.database.repositories.choix_repo import ChoixRepository
        self._filieres = FiliereRepository(use_session_db=True).get_all()
        existing = ChoixRepository().get_by_candidat(self._candidat_id)
        chosen_ids = [c.filiere_id for c in existing]

        self._list_choices.clear()
        for c in existing:
            for f in self._filieres:
                if f.filiere_id == c.filiere_id:
                    self._list_choices.addItem(f.filiere_nom)
                    break

        self._list_available.clear()
        for f in self._filieres:
            if f.filiere_id not in chosen_ids:
                self._list_available.addItem(f.filiere_nom)

    def _on_add(self):
        row = self._list_available.currentRow()
        if row < 0:
            return
        if self._list_choices.count() >= AppState.max_choices:
            return
        item = self._list_available.takeItem(row)
        self._list_choices.addItem(item)

    def _on_remove(self):
        row = self._list_choices.currentRow()
        if row < 0:
            return
        item = self._list_choices.takeItem(row)
        self._list_available.addItem(item)

    def _on_up(self):
        row = self._list_choices.currentRow()
        if row > 0:
            item = self._list_choices.takeItem(row)
            self._list_choices.insertItem(row - 1, item)
            self._list_choices.setCurrentRow(row - 1)

    def _on_down(self):
        row = self._list_choices.currentRow()
        if 0 <= row < self._list_choices.count() - 1:
            item = self._list_choices.takeItem(row)
            self._list_choices.insertItem(row + 1, item)
            self._list_choices.setCurrentRow(row + 1)

    def _on_ok(self):
        from app.database.repositories.filiere_repo import FiliereRepository
        from app.database.repositories.choix_repo import ChoixRepository
        # Build filiere_id list in order from list_choices
        filiere_by_name = {f.filiere_nom: f.filiere_id
                           for f in FiliereRepository(use_session_db=True).get_all()}
        ids = []
        for i in range(self._list_choices.count()):
            name = self._list_choices.item(i).text()
            fid = filiere_by_name.get(name)
            if fid:
                ids.append(fid)
        ChoixRepository().save_choices(self._candidat_id, ids)
        self.accept()
