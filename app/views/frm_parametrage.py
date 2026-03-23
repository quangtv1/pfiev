from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QTabWidget, QWidget, QTableWidget, QTableWidgetItem,
    QHBoxLayout, QPushButton, QDialogButtonBox, QMessageBox
)
from PySide6.QtCore import Qt
from app.localization import L


class MatiereTab(QWidget):
    def __init__(self, parent=None):
        super().__init__(parent)
        self._matieres = []
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QHBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(3)
        self._table.setHorizontalHeaderLabels(["Code", "Tên", L.get("LblCoefficient")])
        self._table.setSelectionBehavior(QTableWidget.SelectRows)
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)

        btn_layout = QVBoxLayout()
        self._btn_add = QPushButton(L.get("BtnAjouter"))
        self._btn_edit = QPushButton(L.get("BtnModifier"))
        self._btn_del = QPushButton(L.get("BtnEffacer"))
        self._btn_add.clicked.connect(self._on_add)
        self._btn_edit.clicked.connect(self._on_edit)
        self._btn_del.clicked.connect(self._on_delete)
        btn_layout.addWidget(self._btn_add)
        btn_layout.addWidget(self._btn_edit)
        btn_layout.addWidget(self._btn_del)
        btn_layout.addStretch()
        layout.addLayout(btn_layout)

    def _load(self):
        from app.database.repositories.matiere_repo import MatiereRepository
        self._matieres = MatiereRepository().get_all()
        self._table.setRowCount(len(self._matieres))
        for r, m in enumerate(self._matieres):
            self._table.setItem(r, 0, QTableWidgetItem(m.matiere_code))
            self._table.setItem(r, 1, QTableWidgetItem(m.matiere_nom))
            self._table.setItem(r, 2, QTableWidgetItem(str(m.matiere_coefficient)))
        self._table.resizeColumnsToContents()

    def _on_add(self):
        from app.views.frm_ajouter_matiere import FrmAjouterMatiere
        dlg = FrmAjouterMatiere(self)
        if dlg.exec() == QDialog.Accepted:
            self._load()

    def _on_edit(self):
        row = self._table.currentRow()
        if row < 0:
            return
        from app.views.frm_ajouter_matiere import FrmAjouterMatiere
        dlg = FrmAjouterMatiere(self, self._matieres[row])
        if dlg.exec() == QDialog.Accepted:
            self._load()

    def _on_delete(self):
        row = self._table.currentRow()
        if row < 0:
            return
        reply = QMessageBox.question(self, "", L.get("MsgConfirmDelete"))
        if reply == QMessageBox.Yes:
            try:
                from app.database.repositories.matiere_repo import MatiereRepository
                MatiereRepository().delete(self._matieres[row].matiere_id)
                self._load()
            except Exception:
                QMessageBox.warning(self, "", L.get("MsgDeleteFkError"))


class EtablissementTab(QWidget):
    def __init__(self, parent=None):
        super().__init__(parent)
        self._etabs = []
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QHBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(2)
        self._table.setHorizontalHeaderLabels(["Code", "Tên"])
        self._table.setSelectionBehavior(QTableWidget.SelectRows)
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)

        btn_layout = QVBoxLayout()
        self._btn_add = QPushButton(L.get("BtnAjouter"))
        self._btn_edit = QPushButton(L.get("BtnModifier"))
        self._btn_del = QPushButton(L.get("BtnEffacer"))
        self._btn_add.clicked.connect(self._on_add)
        self._btn_edit.clicked.connect(self._on_edit)
        self._btn_del.clicked.connect(self._on_delete)
        btn_layout.addWidget(self._btn_add)
        btn_layout.addWidget(self._btn_edit)
        btn_layout.addWidget(self._btn_del)
        btn_layout.addStretch()
        layout.addLayout(btn_layout)

    def _load(self):
        from app.database.repositories.etablissement_repo import EtablissementRepository
        self._etabs = EtablissementRepository().get_all()
        self._table.setRowCount(len(self._etabs))
        for r, e in enumerate(self._etabs):
            self._table.setItem(r, 0, QTableWidgetItem(e.etab_code))
            self._table.setItem(r, 1, QTableWidgetItem(e.etab_nom))
        self._table.resizeColumnsToContents()

    def _on_add(self):
        from app.views.frm_ajouter_etab import FrmAjouterEtab
        dlg = FrmAjouterEtab(self)
        if dlg.exec() == QDialog.Accepted:
            self._load()

    def _on_edit(self):
        row = self._table.currentRow()
        if row < 0:
            return
        from app.views.frm_ajouter_etab import FrmAjouterEtab
        dlg = FrmAjouterEtab(self, self._etabs[row])
        if dlg.exec() == QDialog.Accepted:
            self._load()

    def _on_delete(self):
        row = self._table.currentRow()
        if row < 0:
            return
        reply = QMessageBox.question(self, "", L.get("MsgConfirmDelete"))
        if reply == QMessageBox.Yes:
            try:
                from app.database.repositories.etablissement_repo import EtablissementRepository
                EtablissementRepository().delete(self._etabs[row].etab_id)
                self._load()
            except Exception:
                QMessageBox.warning(self, "", L.get("MsgDeleteFkError"))


class FiliereTab(QWidget):
    def __init__(self, parent=None):
        super().__init__(parent)
        self._filieres = []
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QHBoxLayout(self)
        self._table = QTableWidget()
        self._table.setColumnCount(4)
        self._table.setHorizontalHeaderLabels(["Code", "Tên", L.get("ColEtablissement"), L.get("LblNbPlace")])
        self._table.setSelectionBehavior(QTableWidget.SelectRows)
        self._table.setEditTriggers(QTableWidget.NoEditTriggers)
        self._table.horizontalHeader().setStretchLastSection(True)
        layout.addWidget(self._table)

        btn_layout = QVBoxLayout()
        self._btn_add = QPushButton(L.get("BtnAjouter"))
        self._btn_edit = QPushButton(L.get("BtnModifier"))
        self._btn_del = QPushButton(L.get("BtnEffacer"))
        self._btn_add.clicked.connect(self._on_add)
        self._btn_edit.clicked.connect(self._on_edit)
        self._btn_del.clicked.connect(self._on_delete)
        btn_layout.addWidget(self._btn_add)
        btn_layout.addWidget(self._btn_edit)
        btn_layout.addWidget(self._btn_del)
        btn_layout.addStretch()
        layout.addLayout(btn_layout)

    def _load(self):
        from app.database.repositories.filiere_repo import FiliereRepository
        self._filieres = FiliereRepository().get_all_with_etab()
        self._table.setRowCount(len(self._filieres))
        for r, row in enumerate(self._filieres):
            self._table.setItem(r, 0, QTableWidgetItem(row.get("FiliereCode") or ""))
            self._table.setItem(r, 1, QTableWidgetItem(row.get("FiliereNom") or ""))
            self._table.setItem(r, 2, QTableWidgetItem(row.get("EtabNom") or ""))
            self._table.setItem(r, 3, QTableWidgetItem(str(row.get("NbPlace") or 0)))
        self._table.resizeColumnsToContents()

    def _on_add(self):
        from app.views.frm_ajouter_filiere import FrmAjouterFiliere
        dlg = FrmAjouterFiliere(self)
        if dlg.exec() == QDialog.Accepted:
            self._load()

    def _on_edit(self):
        row = self._table.currentRow()
        if row < 0:
            return
        from app.database.repositories.filiere_repo import FiliereRepository
        from app.views.frm_ajouter_filiere import FrmAjouterFiliere
        filieres = FiliereRepository().get_all()
        if row < len(filieres):
            dlg = FrmAjouterFiliere(self, filieres[row])
            if dlg.exec() == QDialog.Accepted:
                self._load()

    def _on_delete(self):
        row = self._table.currentRow()
        if row < 0:
            return
        reply = QMessageBox.question(self, "", L.get("MsgConfirmDelete"))
        if reply == QMessageBox.Yes:
            try:
                from app.database.repositories.filiere_repo import FiliereRepository
                filieres = FiliereRepository().get_all()
                if row < len(filieres):
                    FiliereRepository().delete(filieres[row].filiere_id)
                    self._load()
            except Exception:
                QMessageBox.warning(self, "", L.get("MsgDeleteFkError"))


class FrmParametrage(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("TitleParametrage"))
        self.resize(600, 420)
        self._setup_ui()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        self._tabs = QTabWidget()
        self._tab_matieres = MatiereTab()
        self._tab_etabs = EtablissementTab()
        self._tab_filieres = FiliereTab()
        self._tabs.addTab(self._tab_matieres, L.get("TabMatieres"))
        self._tabs.addTab(self._tab_etabs, L.get("TabEtablissements"))
        self._tabs.addTab(self._tab_filieres, L.get("TabFilieres"))
        layout.addWidget(self._tabs)

        btn_layout = QHBoxLayout()
        btn_places = QPushButton(L.get("BtnFixerNbrePlaces"))
        btn_nb = QPushButton(L.get("TitleFixerNbFiliere"))
        btn_close = QPushButton(L.get("BtnClose"))
        btn_places.clicked.connect(self._on_fixer_places)
        btn_nb.clicked.connect(self._on_fixer_nb)
        btn_close.clicked.connect(self.accept)
        btn_layout.addWidget(btn_places)
        btn_layout.addWidget(btn_nb)
        btn_layout.addStretch()
        btn_layout.addWidget(btn_close)
        layout.addLayout(btn_layout)

    def _on_fixer_places(self):
        from app.views.frm_fixer_nbre_place import FrmFixerNbrePlace
        dlg = FrmFixerNbrePlace(self)
        if dlg.exec() == QDialog.Accepted:
            self._tab_filieres._load()

    def _on_fixer_nb(self):
        from app.views.frm_fixer_nb_filiere import FrmFixerNbFiliere
        FrmFixerNbFiliere(self).exec()
