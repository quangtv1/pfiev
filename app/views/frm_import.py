from pathlib import Path
from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QHBoxLayout, QLabel, QLineEdit,
    QPushButton, QDialogButtonBox, QFileDialog, QMessageBox, QProgressBar
)
from app.localization import L


class FrmImport(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleImport"))
        self.setFixedSize(480, 150)
        self._file_path: str = ""
        self._setup_ui()

    def _setup_ui(self):
        layout = QVBoxLayout(self)

        path_row = QHBoxLayout()
        self._txt_path = QLineEdit()
        self._txt_path.setReadOnly(True)
        btn_browse = QPushButton(L.get("BtnBrowseFile"))
        btn_browse.clicked.connect(self._on_browse)
        path_row.addWidget(self._txt_path)
        path_row.addWidget(btn_browse)
        layout.addLayout(path_row)

        btns = QDialogButtonBox(QDialogButtonBox.Ok | QDialogButtonBox.Cancel)
        btns.accepted.connect(self._on_import)
        btns.rejected.connect(self.reject)
        layout.addWidget(btns)

    def _on_browse(self):
        path, _ = QFileDialog.getOpenFileName(
            self, L.get("TitleSelectExcelFile"), "", "Excel Files (*.xlsx)"
        )
        if path:
            self._file_path = path
            self._txt_path.setText(path)

    def _on_import(self):
        if not self._file_path or not Path(self._file_path).exists():
            QMessageBox.warning(self, "", L.get("MsgSelectFile"))
            return
        try:
            from app.services.excel_service import ExcelService
            from app.database.repositories.candidat_repo import CandidatRepository
            from app.database.repositories.etablissement_repo import EtablissementRepository
            from app.database.repositories.filiere_repo import FiliereRepository
            from app.database.repositories.choix_repo import ChoixRepository
            from app.models.candidat import Candidat

            rows = ExcelService.import_candidats(self._file_path)

            # Build lookup maps
            etabs = {e.etab_code: e.etab_id
                     for e in EtablissementRepository(use_session_db=True).get_all()}
            filieres = {f.filiere_code: f.filiere_id
                        for f in FiliereRepository(use_session_db=True).get_all()
                        if f.filiere_code}

            candidat_repo = CandidatRepository()
            choix_repo = ChoixRepository()
            imported = 0
            skipped = 0
            for row in rows:
                if not row.nom:
                    skipped += 1
                    continue
                etab_id = etabs.get(row.etab_code, 0)
                c = Candidat(
                    nom=row.nom,
                    nom_intermediaire=row.nom_intermediaire,
                    prenom=row.prenom,
                    date_de_naissance=row.date_de_naissance or None,
                    sexe=row.sexe or "M",
                    candidat_statut=row.statut or "I",
                    langue=row.langue or "fr",
                    etab_id=etab_id
                )
                new_id = candidat_repo.add(c)

                # Save filiere choices in priority order
                if row.filiere_choices and new_id:
                    filiere_ids = [
                        filieres[fcode]
                        for fcode, _ in row.filiere_choices
                        if fcode in filieres
                    ]
                    if filiere_ids:
                        choix_repo.save_choices(new_id, filiere_ids)

                imported += 1

            QMessageBox.information(
                self, "", L.fmt("MsgImportDone", imported, skipped)
            )
            self.accept()
        except Exception as e:
            QMessageBox.critical(self, "", f"{L.get('MsgImportError')}: {e}")
