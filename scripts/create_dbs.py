"""Create config.db and template.db SQLite databases for OrientationPFIEV."""
import sqlite3
from pathlib import Path

DATA_DIR = Path(__file__).parent.parent / "data"
DATA_DIR.mkdir(exist_ok=True)

CONFIG_DB = DATA_DIR / "config.db"
TEMPLATE_DB = DATA_DIR / "template.mdb"

# --- Shared table DDL (appears in both DBs) ---
SHARED_DDL = """
CREATE TABLE IF NOT EXISTS Matiere (
    MatiereID INTEGER PRIMARY KEY AUTOINCREMENT,
    MatiereNom TEXT NOT NULL,
    MatiereCoefficient REAL DEFAULT 1.0,
    MatiereCode TEXT DEFAULT ''
);
CREATE TABLE IF NOT EXISTS Etablissement (
    EtabID INTEGER PRIMARY KEY AUTOINCREMENT,
    EtabNom TEXT NOT NULL,
    EtabCode TEXT DEFAULT ''
);
CREATE TABLE IF NOT EXISTS Filiere (
    FiliereID INTEGER PRIMARY KEY AUTOINCREMENT,
    FiliereNom TEXT NOT NULL,
    FiliereCode TEXT DEFAULT '',
    EtabID INTEGER,
    NbPlace INTEGER DEFAULT 0,
    FOREIGN KEY (EtabID) REFERENCES Etablissement(EtabID)
);
"""

CONFIG_EXTRA_DDL = """
CREATE TABLE IF NOT EXISTS DataPath (
    PathID INTEGER PRIMARY KEY AUTOINCREMENT,
    FilePath TEXT NOT NULL
);
"""

SESSION_DDL = """
CREATE TABLE IF NOT EXISTS Candidat (
    CandidatID INTEGER PRIMARY KEY AUTOINCREMENT,
    Nom TEXT DEFAULT '',
    NomIntermediaire TEXT DEFAULT '',
    Prenom TEXT DEFAULT '',
    DateDeNaissance TEXT,
    Sexe TEXT DEFAULT '',
    CandidatStatut TEXT DEFAULT 'I',
    Langue TEXT DEFAULT 'fr',
    EtabID INTEGER DEFAULT 0,
    CandidatMoyenne REAL,
    CandidatClassement INTEGER,
    anonymat TEXT DEFAULT ''
);
CREATE TABLE IF NOT EXISTS Concours (
    ConcoursID INTEGER PRIMARY KEY AUTOINCREMENT,
    Annee INTEGER DEFAULT 0,
    MoyenneMin REAL DEFAULT 0.0
);
CREATE TABLE IF NOT EXISTS Note (
    NoteID INTEGER PRIMARY KEY AUTOINCREMENT,
    CandidatID INTEGER,
    MatiereID INTEGER,
    Note TEXT DEFAULT '0',
    FOREIGN KEY (CandidatID) REFERENCES Candidat(CandidatID),
    FOREIGN KEY (MatiereID) REFERENCES Matiere(MatiereID)
);
CREATE TABLE IF NOT EXISTS Choix (
    ChoixID INTEGER PRIMARY KEY AUTOINCREMENT,
    CandidatID INTEGER,
    FiliereID INTEGER,
    ChoixOrdre INTEGER DEFAULT 1,
    ChoixAdmis INTEGER DEFAULT 0,
    FiliereNbPlace INTEGER DEFAULT 0,
    FOREIGN KEY (CandidatID) REFERENCES Candidat(CandidatID),
    FOREIGN KEY (FiliereID) REFERENCES Filiere(FiliereID)
);
"""


def create_config_db():
    conn = sqlite3.connect(str(CONFIG_DB))
    conn.executescript(SHARED_DDL + CONFIG_EXTRA_DDL)
    conn.commit()
    conn.close()
    print(f"Created: {CONFIG_DB}")


def create_template_db():
    # template.mdb is a SQLite file with .mdb extension — no Access driver needed
    conn = sqlite3.connect(str(TEMPLATE_DB))
    conn.executescript(SHARED_DDL + SESSION_DDL)
    conn.commit()
    conn.close()
    print(f"Created: {TEMPLATE_DB}")


if __name__ == "__main__":
    create_config_db()
    create_template_db()
    print("Done.")
