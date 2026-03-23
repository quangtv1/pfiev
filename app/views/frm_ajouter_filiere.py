from typing import Optional
from PySide6.QtWidgets import (
    QDialog, QFormLayout, QLineEdit, QComboBox, QSpinBox,
    QDialogButtonBox, QMessageBox
)
from app.localization import L
from app.models.filiere import Filiere


class FrmAjouterFiliere(QDialog):
    def __init__(self, parent=None, filiere: Optional[Filiere] = None):
        super().__init__(parent)
        self._filiere = filiere
        self._etabs = []
        title = L.get("TitleModifierFiliere") if filiere else L.get("TitleAjouterFiliere")
        self.setWindowTitle(title)
        self.setFixedSize(320, 180)
        self._setup_ui()
        self._load_etabs()
        if filiere:
            self._txt_nom.setText(filiere.filiere_nom)
            self._txt_code.setText(filiere.filiere_code)
            self._spin_nb.setValue(filiere.nb_place)
            # Select the matching etab in combo
            for i, e in enumerate(self._etabs):
                if e.etab_id == filiere.etab_id:
                    self._combo_etab.setCurrentIndex(i)
                    break

    def _setup_ui(self):
        layout = QFormLayout(self)
        self._txt_nom = QLineEdit()
        self._txt_code = QLineEdit()
        self._combo_etab = QComboBox()
        self._spin_nb = QSpinBox()
        self._spin_nb.setRange(0, 9999)

        layout.addRow(L.get("LblNom"), self._txt_nom)
        layout.addRow(L.get("LblCode"), self._txt_code)
        layout.addRow(L.get("LblEtablissement"), self._combo_etab)
        layout.addRow(L.get("LblNbPlace"), self._spin_nb)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _load_etabs(self):
        from app.database.repositories.etablissement_repo import EtablissementRepository
        self._etabs = EtablissementRepository().get_all()
        self._combo_etab.clear()
        for e in self._etabs:
            self._combo_etab.addItem(e.etab_nom)

    def _on_ok(self):
        nom = self._txt_nom.text().strip()
        if not nom:
            QMessageBox.warning(self, "", L.get("MsgFieldRequired"))
            return
        etab_id = 0
        if self._etabs and self._combo_etab.currentIndex() >= 0:
            etab_id = self._etabs[self._combo_etab.currentIndex()].etab_id

        from app.database.repositories.filiere_repo import FiliereRepository
        f = Filiere(
            filiere_id=self._filiere.filiere_id if self._filiere else 0,
            filiere_nom=nom,
            filiere_code=self._txt_code.text().strip(),
            etab_id=etab_id,
            nb_place=self._spin_nb.value()
        )
        repo = FiliereRepository()
        if self._filiere:
            repo.update(f)
        else:
            repo.add(f)
        self.accept()
