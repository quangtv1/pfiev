from pathlib import Path
from PySide6.QtWidgets import QWidget, QLabel, QProgressBar, QVBoxLayout
from PySide6.QtCore import QTimer, Qt
from PySide6.QtGui import QPixmap


def _res(name: str) -> str:
    import sys
    base = getattr(sys, "_MEIPASS", Path(__file__).parent.parent.parent)
    return str(Path(base) / "resources" / name)


class FrmAccueil(QWidget):
    MAX_TICKS = 15

    def __init__(self):
        super().__init__()
        self._tick = 0
        self._lang_dlg = None
        self._setup_ui()
        self._timer = QTimer(self)
        self._timer.setInterval(100)
        self._timer.timeout.connect(self._on_tick)

    def _setup_ui(self):
        self.setWindowFlags(Qt.FramelessWindowHint)
        self.setFixedSize(480, 320)

        # Center on screen
        from PySide6.QtGui import QScreen
        from PySide6.QtWidgets import QApplication
        screen = QApplication.primaryScreen()
        if screen:
            rect = screen.availableGeometry()
            self.move(
                rect.center().x() - self.width() // 2,
                rect.center().y() - self.height() // 2
            )

        layout = QVBoxLayout(self)
        layout.setContentsMargins(0, 0, 0, 0)
        layout.setSpacing(0)

        self._logo = QLabel()
        self._logo.setAlignment(Qt.AlignCenter)
        pix = QPixmap(_res("logo.jpg"))
        if not pix.isNull():
            self._logo.setPixmap(pix.scaled(480, 295, Qt.KeepAspectRatio, Qt.SmoothTransformation))
        else:
            self._logo.setText("OrientationPFIEV")
            self._logo.setStyleSheet("font-size: 24px; font-weight: bold;")
            self._logo.setFixedHeight(295)
        layout.addWidget(self._logo)

        self._progress = QProgressBar()
        self._progress.setRange(0, self.MAX_TICKS)
        self._progress.setValue(0)
        self._progress.setFixedHeight(10)
        self._progress.setTextVisible(False)
        layout.addWidget(self._progress)

    def showEvent(self, event):
        super().showEvent(event)
        self._timer.start()

    def _on_tick(self):
        self._tick += 1
        self._progress.setValue(self._tick)
        if self._tick >= self.MAX_TICKS:
            self._timer.stop()
            from app.views.frm_lancement import FrmLancement
            self._lang_dlg = FrmLancement()
            self._lang_dlg.show()
            self.hide()  # NOT close() — closing main widget exits QApplication
