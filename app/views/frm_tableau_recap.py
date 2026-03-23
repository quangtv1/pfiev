from PySide6.QtWidgets import (
    QWidget, QVBoxLayout, QHBoxLayout, QTabWidget,
    QTableWidget, QTableWidgetItem, QPushButton
)
from app.localization import L


class FrmTableauRecap(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle(L.get("TitleTableauRecap"))
        self.resize(700, 500)
        self._setup_ui()
        self._load()

    def _setup_ui(self):
        layout = QVBoxLayout(self)
        tabs = QTabWidget()

        # Internes tab
        self._tbl_int = QTableWidget()
        self._tbl_int.setColumnCount(4)
        self._tbl_int.setHorizontalHeaderLabels(
            [L.get("ColFiliere"), L.get("ColPlaces"), L.get("ColNbAdmis"), L.get("ColMoyenneSpe")]
        )
        self._tbl_int.setEditTriggers(QTableWidget.NoEditTriggers)
        tabs.addTab(self._tbl_int, L.get("TabInternes"))

        # Externes tab
        self._tbl_ext = QTableWidget()
        self._tbl_ext.setColumnCount(4)
        self._tbl_ext.setHorizontalHeaderLabels(
            [L.get("ColFiliere"), L.get("ColPlaces"), L.get("ColNbAdmis"), L.get("ColMoyenneSpe")]
        )
        self._tbl_ext.setEditTriggers(QTableWidget.NoEditTriggers)
        tabs.addTab(self._tbl_ext, L.get("TabExternes"))
        layout.addWidget(tabs)

        btn_row = QHBoxLayout()
        for title, handler in [
            (L.get("TitleStat"), self._open_stat),
            (L.get("TitleStat1"), self._open_stat1),
            (L.get("TitleStat6"), self._open_stat6),
            (L.get("TitleStatEtab"), self._open_stat_etab),
            (L.get("TitleStatMoyenne"), self._open_stat_moy),
            (L.get("TitleMoyenneSpe"), self._open_moy_spe),
            (L.get("TitleGraphe"), self._open_graphe),
        ]:
            btn = QPushButton(title)
            btn.clicked.connect(handler)
            btn_row.addWidget(btn)
        layout.addLayout(btn_row)

    def _load(self):
        from app.database.repositories.resultats_repo import ResultatsRepository
        repo = ResultatsRepository()

        def fill(table, rows):
            table.setRowCount(len(rows))
            for r, row in enumerate(rows):
                table.setItem(r, 0, QTableWidgetItem(row.get("FiliereNom") or ""))
                table.setItem(r, 1, QTableWidgetItem(str(row.get("NbPlace") or 0)))
                table.setItem(r, 2, QTableWidgetItem(str(row.get("NbAdmis") or 0)))
                moy = row.get("MoyenneSpe")
                table.setItem(r, 3, QTableWidgetItem(f"{moy:.2f}" if moy else ""))
            table.resizeColumnsToContents()

        fill(self._tbl_int, repo.get_tableau_recap_internes())
        fill(self._tbl_ext, repo.get_tableau_recap_externes())

    def _open_stat(self):
        from app.views.frm_stat import FrmStat
        FrmStat(self).exec()

    def _open_stat1(self):
        from app.views.frm_stat1 import FrmStat1
        FrmStat1(self).exec()

    def _open_stat6(self):
        from app.views.frm_stat6 import FrmStat6
        FrmStat6(self).exec()

    def _open_stat_etab(self):
        from app.views.frm_stat_etab import FrmStatEtab
        FrmStatEtab(self).exec()

    def _open_stat_moy(self):
        from app.views.frm_stat_moyenne import FrmStatMoyenne
        FrmStatMoyenne(self).exec()

    def _open_moy_spe(self):
        from app.views.frm_moyenne_spe import FrmMoyenneSpe
        FrmMoyenneSpe(self).exec()

    def _open_graphe(self):
        from app.views.frm_resultats_graphe import FrmResultatsGraphe
        w = FrmResultatsGraphe()
        w.show()
