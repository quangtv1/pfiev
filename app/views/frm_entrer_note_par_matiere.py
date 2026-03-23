"""Note entry form with 2 tabs: by subject and by candidate (like VB frmEntrerNoteParMatiere)."""
from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QHBoxLayout, QTabWidget, QWidget,
    QComboBox, QTableWidget, QTableWidgetItem, QPushButton, QLabel,
    QSplitter
)
from PySide6.QtCore import Qt
from app.localization import L


class FrmEntrerNoteParMatiere(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleEntrerNoteParMatiere"))
        self.resize(700, 500)
        self._matieres = []
        self._setup_ui()
        self._load_data()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._tabs = QTabWidget()
        layout.addWidget(self._tabs)

        self._build_tab_by_matiere()
        self._build_tab_by_candidat()

    # ── Tab 1: by subject ────────────────────────────────────────────────────
    def _build_tab_by_matiere(self):
        tab = QWidget()
        v = QVBoxLayout(tab)

        row = QHBoxLayout()
        row.addWidget(QLabel(L.get("ColMatiere") + ":"))
        self._cmb_matiere = QComboBox()
        self._cmb_matiere.currentIndexChanged.connect(self._refresh_by_matiere)
        row.addWidget(self._cmb_matiere)
        row.addStretch()
        v.addLayout(row)

        self._tbl_mat = QTableWidget()
        self._tbl_mat.setColumnCount(5)
        self._tbl_mat.setHorizontalHeaderLabels([
            "ID", L.get("ColAnonymat"), L.get("ColNom"),
            L.get("ColNomIntermediaire"), L.get("ColNote")
        ])
        self._tbl_mat.setSelectionBehavior(QTableWidget.SelectRows)
        self._tbl_mat.setEditTriggers(QTableWidget.NoEditTriggers)
        self._tbl_mat.horizontalHeader().setStretchLastSection(True)
        self._tbl_mat.setColumnHidden(0, True)
        self._tbl_mat.doubleClicked.connect(self._on_edit_by_matiere)
        v.addWidget(self._tbl_mat)

        btns = QHBoxLayout()
        btns.addStretch()
        btn_mod = QPushButton(L.get("BtnModifier"))
        btn_mod.clicked.connect(self._on_edit_by_matiere)
        btn_close = QPushButton(L.get("BtnClose"))
        btn_close.clicked.connect(self.accept)
        btns.addWidget(btn_mod)
        btns.addWidget(btn_close)
        v.addLayout(btns)

        self._tabs.addTab(tab, L.get("TabNoteParMatiere"))

    # ── Tab 2: by candidate ──────────────────────────────────────────────────
    def _build_tab_by_candidat(self):
        tab = QWidget()
        v = QVBoxLayout(tab)

        splitter = QSplitter(Qt.Vertical)

        self._tbl_cand = QTableWidget()
        self._tbl_cand.setColumnCount(4)
        self._tbl_cand.setHorizontalHeaderLabels([
            "ID", L.get("ColAnonymat"), L.get("ColNom"), L.get("ColNomIntermediaire")
        ])
        self._tbl_cand.setSelectionBehavior(QTableWidget.SelectRows)
        self._tbl_cand.setEditTriggers(QTableWidget.NoEditTriggers)
        self._tbl_cand.setColumnHidden(0, True)
        self._tbl_cand.selectionModel().selectionChanged.connect(self._on_candidat_selected)
        splitter.addWidget(self._tbl_cand)

        self._tbl_notes = QTableWidget()
        self._tbl_notes.setColumnCount(2)
        self._tbl_notes.setHorizontalHeaderLabels([L.get("ColMatiere"), L.get("ColNote")])
        self._tbl_notes.setEditTriggers(QTableWidget.NoEditTriggers)
        self._tbl_notes.horizontalHeader().setStretchLastSection(True)
        splitter.addWidget(self._tbl_notes)

        v.addWidget(splitter)

        btns = QHBoxLayout()
        btns.addStretch()
        btn_mod = QPushButton(L.get("BtnModifier"))
        btn_mod.clicked.connect(self._on_edit_by_candidat)
        btn_close = QPushButton(L.get("BtnClose"))
        btn_close.clicked.connect(self.accept)
        btns.addWidget(btn_mod)
        btns.addWidget(btn_close)
        v.addLayout(btns)

        self._tabs.addTab(tab, L.get("TabNoteParCandidat"))

    # ── Data loading ─────────────────────────────────────────────────────────
    def _load_data(self):
        from app.database.repositories.matiere_repo import MatiereRepository
        self._matieres = MatiereRepository(use_session_db=True).get_all()
        self._cmb_matiere.clear()
        for m in self._matieres:
            self._cmb_matiere.addItem(m.matiere_nom, m.matiere_id)

        self._load_candidats_tab()
        if self._matieres:
            self._refresh_by_matiere(0)

    def _refresh_by_matiere(self, _idx=None):
        matiere_id = self._cmb_matiere.currentData()
        if matiere_id is None:
            return
        from app.database.session_db import SessionDB
        sql = """
            SELECT c.CandidatID, c.anonymat, c.Nom, c.NomIntermediaire, n.Note
            FROM Candidat c
            LEFT JOIN Note n ON c.CandidatID = n.CandidatID AND n.MatiereID = ?
            ORDER BY c.anonymat
        """
        rows = SessionDB.conn().execute(sql, (matiere_id,)).fetchall()
        self._tbl_mat.setRowCount(len(rows))
        for r, row in enumerate(rows):
            for c, val in enumerate(row):
                self._tbl_mat.setItem(r, c, QTableWidgetItem(str(val or "")))
        self._tbl_mat.resizeColumnsToContents()

    def _load_candidats_tab(self):
        from app.database.session_db import SessionDB
        rows = SessionDB.conn().execute(
            "SELECT CandidatID, anonymat, Nom, NomIntermediaire FROM Candidat ORDER BY anonymat"
        ).fetchall()
        self._tbl_cand.setRowCount(len(rows))
        for r, row in enumerate(rows):
            for c, val in enumerate(row):
                self._tbl_cand.setItem(r, c, QTableWidgetItem(str(val or "")))
        self._tbl_cand.resizeColumnsToContents()

    def _on_candidat_selected(self):
        row = self._tbl_cand.currentRow()
        if row < 0:
            self._tbl_notes.setRowCount(0)
            return
        cid = int(self._tbl_cand.item(row, 0).text())
        from app.database.repositories.note_repo import NoteRepository
        notes = {n.matiere_id: n.note_value for n in NoteRepository().get_by_candidat(cid)}
        self._tbl_notes.setRowCount(len(self._matieres))
        for r, m in enumerate(self._matieres):
            self._tbl_notes.setItem(r, 0, QTableWidgetItem(m.matiere_nom))
            self._tbl_notes.setItem(r, 1, QTableWidgetItem(str(notes.get(m.matiere_id, ""))))
        self._tbl_notes.resizeColumnsToContents()

    # ── Edit actions ─────────────────────────────────────────────────────────
    def _on_edit_by_matiere(self):
        row = self._tbl_mat.currentRow()
        if row < 0:
            return
        cid = int(self._tbl_mat.item(row, 0).text())
        from app.views.frm_note_matiere import FrmNoteMatiere
        if FrmNoteMatiere(cid, self).exec():
            self._refresh_by_matiere()

    def _on_edit_by_candidat(self):
        row = self._tbl_cand.currentRow()
        if row < 0:
            return
        cid = int(self._tbl_cand.item(row, 0).text())
        from app.views.frm_note_matiere import FrmNoteMatiere
        if FrmNoteMatiere(cid, self).exec():
            self._on_candidat_selected()
