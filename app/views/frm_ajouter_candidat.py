from typing import Optional
from PySide6.QtWidgets import (
    QDialog, QFormLayout, QLineEdit, QComboBox, QDateEdit,
    QDialogButtonBox, QMessageBox
)
from PySide6.QtCore import QDate
from app.localization import L
from app.models.candidat import Candidat


class FrmAjouterCandidat(QDialog):
    def __init__(self, parent=None, candidat_id: Optional[int] = None):
        super().__init__(parent)
        self._candidat_id = candidat_id
        self._etabs = []
        title = L.get("TitleAjouterCandidat")
        self.setWindowTitle(title)
        self.setFixedSize(360, 320)
        self._setup_ui()
        self._load_etabs()
        if candidat_id:
            self._load_candidat(candidat_id)

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._txt_nom = QLineEdit()
        self._txt_nom_int = QLineEdit()
        self._txt_prenom = QLineEdit()
        self._date_dob = QDateEdit()
        self._date_dob.setDisplayFormat("dd/MM/yyyy")
        self._date_dob.setCalendarPopup(True)
        self._date_dob.setDate(QDate.currentDate())
        self._combo_sexe = QComboBox()
        self._combo_sexe.addItems(["M", "F"])
        self._combo_statut = QComboBox()
        self._combo_statut.addItems(["I", "E"])
        self._combo_langue = QComboBox()
        self._combo_langue.addItems(["fr", "vi"])
        self._combo_etab = QComboBox()
        self._txt_anonymat = QLineEdit()

        layout.addRow(L.get("LblNom"), self._txt_nom)
        layout.addRow(L.get("LblNomIntermediaire"), self._txt_nom_int)
        layout.addRow(L.get("ColPrenom"), self._txt_prenom)
        layout.addRow(L.get("LblDOB"), self._date_dob)
        layout.addRow(L.get("LblSex"), self._combo_sexe)
        layout.addRow(L.get("LblStatus"), self._combo_statut)
        layout.addRow(L.get("LblLanguage"), self._combo_langue)
        layout.addRow(L.get("LblSchool"), self._combo_etab)
        layout.addRow(L.get("ColAnonymat"), self._txt_anonymat)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _load_etabs(self):
        from app.database.repositories.etablissement_repo import EtablissementRepository
        self._etabs = EtablissementRepository(use_session_db=True).get_all()
        self._combo_etab.clear()
        for e in self._etabs:
            self._combo_etab.addItem(e.etab_nom)

    def _load_candidat(self, candidat_id: int):
        from app.database.repositories.candidat_repo import CandidatRepository
        c = CandidatRepository().get_by_id(candidat_id)
        if not c:
            return
        self._txt_nom.setText(c.nom)
        self._txt_nom_int.setText(c.nom_intermediaire)
        self._txt_prenom.setText(c.prenom)
        if c.date_de_naissance:
            try:
                d = QDate.fromString(c.date_de_naissance, "yyyy-MM-dd")
                if d.isValid():
                    self._date_dob.setDate(d)
            except Exception:
                pass
        idx = self._combo_sexe.findText(c.sexe)
        if idx >= 0:
            self._combo_sexe.setCurrentIndex(idx)
        idx = self._combo_statut.findText(c.candidat_statut)
        if idx >= 0:
            self._combo_statut.setCurrentIndex(idx)
        idx = self._combo_langue.findText(c.langue)
        if idx >= 0:
            self._combo_langue.setCurrentIndex(idx)
        for i, e in enumerate(self._etabs):
            if e.etab_id == c.etab_id:
                self._combo_etab.setCurrentIndex(i)
                break
        self._txt_anonymat.setText(c.anonymat)

    def _on_ok(self):
        nom = self._txt_nom.text().strip()
        if not nom:
            QMessageBox.warning(self, "", L.get("MsgFieldRequired"))
            return
        etab_id = 0
        if self._etabs and self._combo_etab.currentIndex() >= 0:
            etab_id = self._etabs[self._combo_etab.currentIndex()].etab_id
        dob = self._date_dob.date().toString("yyyy-MM-dd")
        from app.database.repositories.candidat_repo import CandidatRepository
        c = Candidat(
            candidat_id=self._candidat_id or 0,
            nom=nom,
            nom_intermediaire=self._txt_nom_int.text().strip(),
            prenom=self._txt_prenom.text().strip(),
            date_de_naissance=dob,
            sexe=self._combo_sexe.currentText(),
            candidat_statut=self._combo_statut.currentText(),
            langue=self._combo_langue.currentText(),
            etab_id=etab_id,
            anonymat=self._txt_anonymat.text().strip()
        )
        repo = CandidatRepository()
        if self._candidat_id:
            repo.update(c)
        else:
            repo.add(c)
        self.accept()
