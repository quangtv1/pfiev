import sys
import matplotlib
matplotlib.use('QtAgg')  # set backend before any other matplotlib import
from pathlib import Path
from PySide6.QtWidgets import QApplication
from app.database.config_db import ConfigDB


def _ensure_databases():
    """Create config.db and template.mdb if they don't exist."""
    data_dir = Path(__file__).parent / "data"
    data_dir.mkdir(exist_ok=True)
    config_db = data_dir / "config.db"
    template_mdb = data_dir / "template.mdb"
    if not config_db.exists() or not template_mdb.exists():
        try:
            from scripts.create_dbs import create_config_db, create_template_db
            if not config_db.exists():
                create_config_db()
            if not template_mdb.exists():
                create_template_db()
        except Exception:
            pass


def main():
    app = QApplication(sys.argv)
    app.setApplicationName("PhanBo-PFIEV")
    app.setStyle("Fusion")

    _ensure_databases()

    config_path = Path(__file__).parent / "data" / "config.db"
    if config_path.exists():
        ConfigDB.initialize(str(config_path))

    # Show splash screen → user picks language → launcher opens
    from app.views.frm_accueil import FrmAccueil
    splash = FrmAccueil()
    splash.show()
    sys.exit(app.exec())


if __name__ == "__main__":
    main()
