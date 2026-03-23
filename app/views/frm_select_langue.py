from pathlib import Path
from PySide6.QtWidgets import QDialog, QHBoxLayout, QVBoxLayout, QLabel, QToolButton
from PySide6.QtGui import QIcon, QPixmap
from PySide6.QtCore import Qt


def _res(name: str) -> str:
    import sys
    base = getattr(sys, "_MEIPASS", Path(__file__).parent.parent.parent)
    return str(Path(base) / "resources" / name)


class FrmSelectLangue(QDialog):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("PhanBo-PFIEV")
        self.setFixedSize(320, 150)
        self._launcher = None
        self._setup_ui()

    def _setup_ui(self):
        layout = QVBoxLayout(self)

        lbl = QLabel("Chọn ngôn ngữ / Choisir la langue :")
        lbl.setAlignment(Qt.AlignCenter)
        layout.addWidget(lbl)

        btn_layout = QHBoxLayout()
        btn_layout.setSpacing(20)

        self._btn_fr = QToolButton()
        self._btn_fr.setText("Tiếng Pháp")
        self._btn_fr.setToolButtonStyle(Qt.ToolButtonTextUnderIcon)
        self._btn_fr.setFixedSize(120, 80)

        self._btn_vi = QToolButton()
        self._btn_vi.setText("Tiếng Việt")
        self._btn_vi.setToolButtonStyle(Qt.ToolButtonTextUnderIcon)
        self._btn_vi.setFixedSize(120, 80)

        fr_pix = QPixmap(_res("french.gif"))
        vi_pix = QPixmap(_res("vietnam.gif"))
        if not fr_pix.isNull():
            self._btn_fr.setIcon(QIcon(fr_pix))
            self._btn_fr.setIconSize(fr_pix.size())
        if not vi_pix.isNull():
            self._btn_vi.setIcon(QIcon(vi_pix))
            self._btn_vi.setIconSize(vi_pix.size())

        self._btn_fr.clicked.connect(lambda: self._select("fr"))
        self._btn_vi.clicked.connect(lambda: self._select("vi"))
        btn_layout.addStretch()
        btn_layout.addWidget(self._btn_fr)
        btn_layout.addWidget(self._btn_vi)
        btn_layout.addStretch()
        layout.addLayout(btn_layout)

    def _select(self, lang: str):
        from app.state import AppState
        from app.localization import L
        AppState.language = lang
        L.set_language(lang)
        from app.views.frm_lancement import FrmLancement
        self._launcher = FrmLancement()
        self._launcher.show()
        self.close()
