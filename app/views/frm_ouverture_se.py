import shutil
from pathlib import Path
from PySide6.QtWidgets import (
    QDialog, QFormLayout, QLineEdit, QPushButton,
    QSpinBox, QDoubleSpinBox, QHBoxLayout, QDialogButtonBox,
    QMessageBox, QFileDialog
)
from app.localization import L
from app.models.concours import Concours


def _data_path(name: str) -> Path:
    import sys
    base = getattr(sys, "_MEIPASS", Path(__file__).parent.parent.parent)
    return Path(base) / "data" / name


class FrmOuvertureSE(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleOuvertureSE"))
        self.setFixedSize(420, 170)
        self._selected_path: str = ""
        self._setup_ui()

    def _setup_ui(self):
        layout = QFormLayout(self)

        path_row = QHBoxLayout()
        self._txt_path = QLineEdit()
        self._txt_path.setReadOnly(True)
        btn_browse = QPushButton(L.get("BtnBrowse"))
        btn_browse.setFixedWidth(40)
        btn_browse.clicked.connect(self._on_browse)
        path_row.addWidget(self._txt_path)
        path_row.addWidget(btn_browse)
        layout.addRow(L.get("LblFichierSession"), path_row)

        self._spin_annee = QSpinBox()
        self._spin_annee.setRange(2000, 2100)
        self._spin_annee.setValue(2024)
        layout.addRow(L.get("LblAnnee"), self._spin_annee)

        self._spin_moy = QDoubleSpinBox()
        self._spin_moy.setRange(0.0, 20.0)
        self._spin_moy.setDecimals(2)
        self._spin_moy.setValue(0.0)
        layout.addRow(L.get("LblMoyMin"), self._spin_moy)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_ok)
        btns.rejected.connect(self.reject)
        layout.addRow(btns)

    def _on_browse(self):
        path, _ = QFileDialog.getSaveFileName(
            self, L.get("TitleNouvelleSession"), "", "Session MDB (*.mdb)"
        )
        if path:
            if not path.lower().endswith(".mdb"):
                path += ".mdb"
            self._selected_path = path
            self._txt_path.setText(path)

    def _on_ok(self):
        if not self._selected_path:
            QMessageBox.warning(self, "", L.get("MsgFieldRequired"))
            return
        try:
            template = _data_path("template.mdb")
            if not template.exists():
                QMessageBox.critical(self, "", f"Template DB not found: {template}")
                return
            shutil.copy(str(template), self._selected_path)

            from app.database.session_db import SessionDB
            SessionDB.open(self._selected_path)

            from app.database.repositories.concours_repo import ConcoursRepository
            ConcoursRepository().set_params(Concours(
                annee=self._spin_annee.value(),
                moyenne_min=self._spin_moy.value()
            ))

            self._copy_config_into_session()

            from app.database.repositories.data_path_repo import DataPathRepository
            DataPathRepository().add(self._selected_path)

            self.accept()
        except Exception as e:
            QMessageBox.critical(self, "", str(e))

    def _copy_config_into_session(self):
        """Copy Matiere, Etablissement, Filiere from config.db into new session.db."""
        from app.database.repositories.matiere_repo import MatiereRepository
        from app.database.repositories.etablissement_repo import EtablissementRepository
        from app.database.repositories.filiere_repo import FiliereRepository

        for m in MatiereRepository().get_all():
            MatiereRepository(use_session_db=True).add(m)
        for e in EtablissementRepository().get_all():
            EtablissementRepository(use_session_db=True).add(e)
        for f in FiliereRepository().get_all():
            FiliereRepository(use_session_db=True).add(f)
