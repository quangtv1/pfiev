from typing import Optional
from PySide6.QtWidgets import (
    QMainWindow, QWidget, QVBoxLayout, QHBoxLayout, QGroupBox,
    QTableWidget, QTableWidgetItem, QLabel, QPushButton, QMessageBox, QDialog
)
from PySide6.QtCore import Qt
from app.localization import L


class FrmSession(QMainWindow):
    COLUMNS = ["CandidatID", "Nom", "NomIntermediaire", "Prenom",
               "DateDeNaissance", "Sexe", "CandidatStatut", "Langue",
               "EtabNom", "anonymat"]

    def __init__(self):
        super().__init__()
        self._navigating = False
        self._results = None
        self._setup_ui()
        self._apply_localization()
        self._load_concours_info()
        self._load_candidats()

    def _setup_ui(self):
        self.resize(900, 600)
        central = QWidget()
        self.setCentralWidget(central)
        main_layout = QHBoxLayout(central)

        # Left: concours info + candidate table
        left = QVBoxLayout()

        self._grp_info = QGroupBox()
        info_layout = QHBoxLayout(self._grp_info)
        self._lbl_annee = QLabel()
        self._lbl_moy_min = QLabel()
        info_layout.addWidget(self._lbl_annee)
        info_layout.addWidget(self._lbl_moy_min)
        info_layout.addStretch()
        left.addWidget(self._grp_info)

        self._table = QTableWidget()
        self._table.setColumnCount(len(self.COLUMNS))
        self._table.setSelectionBehavior(QTableWidget.SelectRows)
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        left.addWidget(self._table)
        main_layout.addLayout(left, stretch=4)

        # Right: action buttons
        right = QVBoxLayout()
        self._btn_add = QPushButton()
        self._btn_edit = QPushButton()
        self._btn_del = QPushButton()
        self._btn_notes = QPushButton()
        self._btn_choix = QPushButton()
        self._btn_param = QPushButton()
        for btn in [self._btn_add, self._btn_edit, self._btn_del,
                    self._btn_notes, self._btn_choix, self._btn_param]:
            right.addWidget(btn)
        right.addStretch()
        main_layout.addLayout(right, stretch=1)

        # Bottom: action bar
        bottom_bar = QHBoxLayout()
        self._btn_import = QPushButton()
        self._btn_export = QPushButton()
        self._btn_lancer = QPushButton()
        self._btn_visualiser = QPushButton()
        self._btn_quitter = QPushButton()
        for btn in [self._btn_import, self._btn_export, self._btn_lancer,
                    self._btn_visualiser, self._btn_quitter]:
            bottom_bar.addWidget(btn)

        # Wrap bottom bar in left layout
        left.addLayout(bottom_bar)

        # Connect signals
        self._btn_add.clicked.connect(self._on_add)
        self._btn_edit.clicked.connect(self._on_edit)
        self._btn_del.clicked.connect(self._on_delete)
        self._btn_notes.clicked.connect(self._on_notes)
        self._btn_choix.clicked.connect(self._on_choix)
        self._btn_param.clicked.connect(self._on_param)
        self._btn_import.clicked.connect(self._on_import)
        self._btn_export.clicked.connect(self._on_export)
        self._btn_lancer.clicked.connect(self._on_lancer)
        self._btn_visualiser.clicked.connect(self._on_visualiser)
        self._btn_quitter.clicked.connect(self._on_quitter)

    def _apply_localization(self):
        self.setWindowTitle(L.get("TitleSession"))
        self._table.setHorizontalHeaderLabels([
            "ID", L.get("ColNom"), L.get("ColNomIntermediaire"), L.get("ColPrenom"),
            L.get("ColDateNaissance"), L.get("ColSexe"), L.get("ColStatut"), L.get("ColLangue"),
            L.get("ColEtablissement"), L.get("ColAnonymat")
        ])
        self._grp_info.setTitle(L.get("TitleSession"))
        self._btn_add.setText(L.get("BtnAjouter"))
        self._btn_edit.setText(L.get("BtnModifier"))
        self._btn_del.setText(L.get("BtnEffacer"))
        self._btn_notes.setText(L.get("BtnFixerNotes"))
        self._btn_choix.setText(L.get("BtnFixerChoix"))
        self._btn_param.setText(L.get("BtnParamConcours"))
        self._btn_import.setText(L.get("BtnImport"))
        self._btn_export.setText(L.get("BtnExport"))
        self._btn_lancer.setText(L.get("BtnLancerTraitement"))
        self._btn_visualiser.setText(L.get("BtnVisualiserResultats"))
        self._btn_quitter.setText(L.get("BtnQuitterSession"))

    def _load_concours_info(self):
        try:
            from app.database.repositories.concours_repo import ConcoursRepository
            c = ConcoursRepository().get_params()
            if c:
                self._lbl_annee.setText(f"{L.get('LblAnnee')}: {c.annee}")
                self._lbl_moy_min.setText(f"{L.get('LblMoyMin')}: {c.moyenne_min}")
        except Exception:
            pass

    def _load_candidats(self):
        try:
            from app.database.repositories.candidat_repo import CandidatRepository
            rows = CandidatRepository().get_session_view()
            self._table.setRowCount(len(rows))
            for r, row in enumerate(rows):
                for c, col in enumerate(self.COLUMNS):
                    val = str(row.get(col) or "")
                    item = QTableWidgetItem(val)
                    self._table.setItem(r, c, item)
            self._table.setColumnHidden(0, True)  # Hide CandidatID
            self._table.resizeColumnsToContents()
        except Exception:
            pass

    def _get_selected_candidat_id(self) -> Optional[int]:
        row = self._table.currentRow()
        if row < 0:
            return None
        item = self._table.item(row, 0)
        try:
            return int(item.text()) if item else None
        except (ValueError, AttributeError):
            return None

    def _on_add(self):
        from app.views.frm_ajouter_candidat import FrmAjouterCandidat
        dlg = FrmAjouterCandidat(self)
        if dlg.exec() == QDialog.Accepted:
            self._load_candidats()

    def _on_edit(self):
        cid = self._get_selected_candidat_id()
        if not cid:
            return
        from app.views.frm_ajouter_candidat import FrmAjouterCandidat
        dlg = FrmAjouterCandidat(self, candidat_id=cid)
        if dlg.exec() == QDialog.Accepted:
            self._load_candidats()

    def _on_delete(self):
        cid = self._get_selected_candidat_id()
        if not cid:
            return
        reply = QMessageBox.question(self, "", L.get("MsgConfirmDelete"))
        if reply == QMessageBox.Yes:
            from app.database.repositories.candidat_repo import CandidatRepository
            CandidatRepository().delete(cid)
            self._load_candidats()

    def _on_notes(self):
        cid = self._get_selected_candidat_id()
        if not cid:
            return
        from app.views.frm_note_matiere import FrmNoteMatiere
        FrmNoteMatiere(cid, self).exec()

    def _on_choix(self):
        cid = self._get_selected_candidat_id()
        if not cid:
            return
        from app.views.frm_fixer_choix import FrmFixerChoix
        FrmFixerChoix(cid, self).exec()

    def _on_param(self):
        from app.views.frm_param_concours import FrmParamConcours
        if FrmParamConcours(self).exec() == QDialog.Accepted:
            self._load_concours_info()

    def _on_import(self):
        from app.views.frm_import import FrmImport
        if FrmImport(self).exec() == QDialog.Accepted:
            self._load_candidats()

    def _on_export(self):
        from app.views.frm_export_excel import FrmExportExcel
        FrmExportExcel(self).exec()

    def _on_lancer(self):
        try:
            from app.database.repositories.choix_repo import ChoixRepository
            from app.database.repositories.candidat_repo import CandidatRepository
            from app.database.config_db import ConfigDB
            from app.services.attribution_service import AttributionService

            ChoixRepository().reset_admis()
            CandidatRepository().compute_averages_and_ranking(ConfigDB.conn())
            AttributionService.run()

            from app.views.frm_resultats import FrmResultats
            self._results = FrmResultats()
            self._results.show()
        except Exception as e:
            QMessageBox.critical(self, "", str(e))

    def _on_visualiser(self):
        try:
            from app.views.frm_resultats import FrmResultats
            self._results = FrmResultats(read_only=True)
            self._results.show()
        except Exception as e:
            QMessageBox.critical(self, "", str(e))

    def _on_quitter(self):
        self._navigating = True
        from app.database.session_db import SessionDB
        SessionDB.close()
        from app.views.frm_lancement import FrmLancement
        self._launcher = FrmLancement()
        self._launcher.show()
        self.close()

    def closeEvent(self, event):
        super().closeEvent(event)
        if not self._navigating:
            from app.database.session_db import SessionDB
            SessionDB.close()
            from app.views.frm_lancement import FrmLancement
            w = FrmLancement()
            w.show()
