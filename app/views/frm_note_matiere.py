from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QTableWidget, QTableWidgetItem,
    QDialogButtonBox, QLineEdit, QMessageBox
)
from app.localization import L


class FrmNoteMatiere(QDialog):
    def __init__(self, candidat_id: int, parent=None):
        super().__init__(parent)
        self._candidat_id = candidat_id
        self._matieres = []
        self.setWindowTitle(L.get("TitleNoteMatiere"))
        self.setFixedSize(350, 400)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(2)
        self._table.setHorizontalHeaderLabels([L.get("ColMatiere"), L.get("ColNote")])
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _load(self):
        from app.database.repositories.matiere_repo import MatiereRepository
        from app.database.repositories.note_repo import NoteRepository
        self._matieres = MatiereRepository(use_session_db=True).get_all()
        notes = {n.matiere_id: n.note_value
                 for n in NoteRepository().get_by_candidat(self._candidat_id)}
        self._table.setRowCount(len(self._matieres))
        for r, m in enumerate(self._matieres):
            self._table.setItem(r, 0, QTableWidgetItem(m.matiere_nom))
            txt = QLineEdit(notes.get(m.matiere_id, ""))
            self._table.setCellWidget(r, 1, txt)
        self._table.resizeColumnsToContents()

    def _on_ok(self):
        from app.database.repositories.note_repo import NoteRepository
        repo = NoteRepository()
        for r, m in enumerate(self._matieres):
            widget = self._table.cellWidget(r, 1)
            val = widget.text().strip() if widget else ""
            if val:
                try:
                    float(val.replace(",", "."))
                except ValueError:
                    QMessageBox.warning(self, "", L.get("MsgOnlyNumbers"))
                    return
                repo.upsert(self._candidat_id, m.matiere_id, val)
        self.accept()
