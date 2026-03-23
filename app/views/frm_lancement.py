from pathlib import Path
from PySide6.QtWidgets import (
    QMainWindow, QWidget, QVBoxLayout, QGroupBox,
    QRadioButton, QComboBox, QPushButton, QHBoxLayout, QMessageBox, QDialog,
    QFileDialog
)
from PySide6.QtCore import Qt
from app.localization import L


class FrmLancement(QMainWindow):
    def __init__(self):
        super().__init__()
        self._navigating = False
        self._session = None
        self._setup_ui()
        self._apply_localization()
        self._load_recent_paths()
        self.setFixedSize(420, 220)

    def _setup_ui(self):
        central = QWidget()
        self.setCentralWidget(central)
        layout = QVBoxLayout(central)

        self._grp = QGroupBox()
        grp_layout = QVBoxLayout(self._grp)
        self._radio_param = QRadioButton()
        self._radio_new = QRadioButton()
        self._radio_existing = QRadioButton()
        self._radio_existing.setChecked(True)
        grp_layout.addWidget(self._radio_param)
        grp_layout.addWidget(self._radio_new)
        grp_layout.addWidget(self._radio_existing)

        path_row = QHBoxLayout()
        self._combo_path = QComboBox()
        self._combo_path.setSizeAdjustPolicy(QComboBox.AdjustToContents)
        self._combo_path.setMinimumWidth(200)
        self._btn_browse_session = QPushButton("...")
        self._btn_browse_session.setFixedWidth(30)
        self._btn_browse_session.clicked.connect(self._on_browse_session)
        path_row.addWidget(self._combo_path)
        path_row.addWidget(self._btn_browse_session)
        grp_layout.addLayout(path_row)
        layout.addWidget(self._grp)

        btn_layout = QHBoxLayout()
        self._btn_ok = QPushButton()
        self._btn_clear = QPushButton()
        self._btn_quit = QPushButton()
        btn_layout.addWidget(self._btn_ok)
        btn_layout.addWidget(self._btn_clear)
        btn_layout.addWidget(self._btn_quit)
        layout.addLayout(btn_layout)

        self._radio_existing.toggled.connect(self._on_existing_toggled)
        self._btn_ok.clicked.connect(self._on_ok)
        self._btn_clear.clicked.connect(self._on_clear)
        self._btn_quit.clicked.connect(self._on_quit)

    def _apply_localization(self):
        self.setWindowTitle(L.get("AppTitle"))
        self._grp.setTitle(L.get("FrameLaunch"))
        self._radio_param.setText(L.get("OptParametrer"))
        self._radio_new.setText(L.get("OptNewSession"))
        self._radio_existing.setText(L.get("OptExistingSession"))
        self._btn_ok.setText(L.get("BtnOk"))
        self._btn_clear.setText(L.get("BtnClearHistory"))
        self._btn_quit.setText(L.get("BtnQuit"))

    def _on_existing_toggled(self, checked: bool):
        self._combo_path.setEnabled(checked)
        self._btn_browse_session.setEnabled(checked)

    def _on_browse_session(self):
        path, _ = QFileDialog.getOpenFileName(
            self, L.get("TitleSelectSession"), "",
            "Session files (*.mdb *.db);;All files (*.*)"
        )
        if path:
            if self._combo_path.findText(path) < 0:
                self._combo_path.insertItem(0, path)
            self._combo_path.setCurrentText(path)

    def _load_recent_paths(self):
        try:
            from app.database.repositories.data_path_repo import DataPathRepository
            paths = DataPathRepository().get_all()
            self._combo_path.clear()
            self._combo_path.addItems(paths)
        except Exception:
            pass
        enabled = self._radio_existing.isChecked()
        self._combo_path.setEnabled(enabled)
        self._btn_browse_session.setEnabled(enabled)

    def _on_ok(self):
        if self._radio_param.isChecked():
            from app.views.frm_parametrage import FrmParametrage
            dlg = FrmParametrage(self)
            dlg.exec()
        elif self._radio_new.isChecked():
            from app.views.frm_ouverture_se import FrmOuvertureSE
            dlg = FrmOuvertureSE(self)
            if dlg.exec() == QDialog.Accepted:
                self._navigating = True
                from app.views.frm_session import FrmSession
                self._session = FrmSession()
                self._session.show()
                self.close()
        else:
            path = self._combo_path.currentText().strip()
            if not path or not Path(path).exists():
                QMessageBox.warning(self, "", L.get("MsgInvalidPath"))
                return
            try:
                from app.database.session_db import SessionDB
                SessionDB.open(path)
                from app.views.frm_session import FrmSession
                self._session = FrmSession()
                self._session.show()
                self._navigating = True
                self.close()
            except Exception as e:
                QMessageBox.critical(self, "", str(e))

    def _on_clear(self):
        reply = QMessageBox.question(self, "", L.get("MsgConfirmClearHistory"))
        if reply == QMessageBox.Yes:
            try:
                from app.database.repositories.data_path_repo import DataPathRepository
                DataPathRepository().clear_all()
            except Exception:
                pass
            self._combo_path.clear()

    def _on_quit(self):
        from PySide6.QtWidgets import QApplication
        self._navigating = True
        QApplication.quit()

    def closeEvent(self, event):
        super().closeEvent(event)
        if not self._navigating:
            from PySide6.QtWidgets import QApplication
            QApplication.quit()
