# Codebase Summary - PhanBo-PFIEV

**Version:** v0.1.3 | **Language:** Python 3.12 + PySide6 (Qt6) | **Last Updated:** 2026-03-23

## Project Overview

PhanBo-PFIEV là ứng dụng desktop (Windows) hỗ trợ quản lý và phân bổ học sinh vào các ngành học dựa trên:
- Kết quả thi cử + điểm trung bình
- Sự lựa chọn của học sinh
- Số lượng chỗ trống của mỗi ngành
- Thuật toán phân bổ tối ưu

Hỗ trợ đa ngôn ngữ: **Tiếng Việt + Tiếng Pháp**

---

## Cấu Trúc Project

```
PhanBo-PFIEV/
├── main.py                          # Entry point
├── requirements.txt                 # Dependencies
├── PhanBo-PFIEV.spec           # PyInstaller config
│
├── app/
│   ├── __init__.py
│   ├── state.py                     # Global app state
│   ├── localization.py              # Multilingual support (L class)
│   │
│   ├── models/                      # Data models (dataclasses)
│   │   ├── candidat.py              # Học sinh (ID, tên, ngày sinh, điểm TB...)
│   │   ├── choix.py                 # Lựa chọn ngành (candidat + filiere)
│   │   ├── concours.py              # Kỳ thi (năm, điểm min)
│   │   ├── etablissement.py         # Trường học (ID, tên)
│   │   ├── filiere.py               # Ngành học (ID, tên, số chỗ)
│   │   ├── matiere.py               # Môn học
│   │   └── note.py                  # Điểm số
│   │
│   ├── database/
│   │   ├── config_db.py             # Singleton: config.db (SQLite)
│   │   ├── session_db.py            # Singleton: session .mdb (SQLite/Access)
│   │   ├── db_adapter.py            # Unified DB wrapper (sqlite3 + pyodbc)
│   │   │
│   │   └── repositories/            # Repository pattern
│   │       ├── candidat_repo.py     # CRUD: Candidat
│   │       ├── choix_repo.py        # CRUD: Choix + attribution logic
│   │       ├── concours_repo.py
│   │       ├── etablissement_repo.py
│   │       ├── filiere_repo.py
│   │       ├── matiere_repo.py
│   │       ├── note_repo.py
│   │       ├── resultats_repo.py    # Detailed query: results view
│   │       └── data_path_repo.py
│   │
│   ├── services/
│   │   ├── attribution_service.py   # Core: allocate students → filiere
│   │   └── excel_service.py         # Import/export candidates, results
│   │
│   └── views/                       # PySide6 UI (Form-based)
│       ├── frm_lancement.py         # Launcher: select language, create session
│       ├── frm_accueil.py           # Dashboard
│       ├── frm_session.py           # Session management
│       ├── frm_ajouter_candidat.py  # Add candidate dialog
│       ├── frm_ajouter_filiere.py   # Add filiere dialog
│       ├── frm_ajouter_matiere.py   # Add subject dialog
│       ├── frm_ajouter_etab.py      # Add establishment dialog
│       ├── frm_import.py            # Import from Excel
│       ├── frm_export_excel.py      # Export results
│       ├── frm_param_concours.py    # Configure concours settings
│       ├── frm_parametrage.py       # System settings
│       ├── frm_note_matiere.py      # Enter grades
│       ├── frm_resultats.py         # View results
│       ├── frm_tableau_recap.py     # Summary table
│       ├── frm_stat*.py             # Statistics & charts (6+ views)
│       ├── frm_fixer_*.py           # Dialogs to fix values
│       ├── frm_changer_spe.py       # Change specialization
│       └── frm_select_langue.py     # Language selector
│
├── resources/
│   ├── strings_vi.json              # Vietnamese localization strings
│   ├── strings_fr.json              # French localization strings
│   └── iconPFEIV.ico                # Application icon
│
├── scripts/
│   └── create_dbs.py                # Initialize config.db + template.mdb
│
├── data/                            # Runtime data
│   ├── config.db                    # SQLite: app configuration
│   └── template.mdb                 # Template for session .mdb files
│
└── .github/workflows/
    └── build.yml                    # GitHub Actions: PyInstaller → .exe
```

---

## Key Components

### AppState (`app/state.py`)
Global state flags:
- `language` - Current language (vi/fr)
- `is_first_open` - First-time user flag
- `session_in_progress` - Active session indicator
- `max_choices` - Max filiere choices per candidate (default: 5)

### Localization (`app/localization.py`)
**L class** - Singleton for i18n:
```python
L.set_language("vi")
text = L.get("key")                # Get translated string
text = L.fmt("key", arg1, arg2)   # Format with args
```
Loads from `resources/strings_{lang}.json`

### Database Adapter (`app/database/db_adapter.py`)
**DbConnection** - Unified wrapper:
- **SQLite**: config.db + session .mdb via sqlite3
- **Access .mdb**: Legacy real Access files via pyodbc (if driver available)

Auto-detects file extension + availability.

### Repository Pattern
All data access through repository classes:
- `CandidatRepository` - Student records + ranking
- `ChoixRepository` - Choices + admission status
- `FilierRepository` - Program data + capacity
- `ResultatsRepository` - Detailed results query

### Attribution Service (`app/services/attribution_service.py`)
**Core Algorithm:**
1. Rank candidates by average (best first)
2. For each candidate, iterate their ordered choices
3. Assign to first filiere with available capacity
4. If no capacity → candidate unassigned

Ported from VB6 moMain.bas / C# original.

### Excel Service (`app/services/excel_service.py`)
- **export_candidats()** - Candidate list → Excel
- **export_resultats()** - Results with admission status → Excel
- **import_from_excel()** - Parse candidates + grades from source file

### UI Layer
**All views inherit from QWidget/QDialog**

**Key Dialogs:**
- `FrmLancement` - Language selection + session creation
- `FrmSession` - Active session management
- `FrmImport` - Excel import wizard
- `FrmResultats` - View + filter results
- `FrmTableauRecap` - Summary table of allocations
- `FrmStat*` - Charts: grade distribution, filiere allocation, etc.

**Navigation Pattern:**
- FrmLancement → FrmSession → FrmAccueil (main menu)
- From FrmAccueil: access all feature dialogs

---

## Data Models

### Candidat
```python
candidat_id: int
nom: str                          # Last name
nom_intermediaire: str            # Middle name
prenom: str                       # First name
date_de_naissance: Optional[str]  # YYYY-MM-DD
sexe: str                         # M/F
candidat_statut: str              # I=internal, E=external
langue: str                       # vi/fr
etab_id: int                      # School ID
candidat_moyenne: Optional[float] # Computed average
candidat_classement: Optional[int]# Rank (1-indexed)
anonymat: str                     # Anonymous ID
```

### Choix
```python
choix_id: int
candidat_id: int
filiere_id: int
choix_ordre: int                  # 1-5 (priority)
choix_admis: bool                 # Allocated?
filiere_nb_place: int             # Snapshot of available seats
```

### Concours
```python
concours_id: int
annee: int                        # Year
moyenne_min: float                # Minimum average for eligibility
```

### Filiere
```python
filiere_id: int
filiere_nom: str
nb_place: int                     # Available seats
etab_filiere: str                 # School/program name
```

---

## Dependencies

| Package | Purpose |
|---------|---------|
| PySide6 ≥ 6.7.0 | Qt6 GUI framework |
| openpyxl ≥ 3.1.0 | Excel import/export |
| matplotlib ≥ 3.9.0 | Charts + statistics |
| pyodbc | Legacy Access .mdb support (optional) |

**Build Tool:**
- PyInstaller → dist/PhanBo-PFIEV/ (Windows .exe)

---

## File Statistics

- **Total Files:** 147
- **Total Tokens:** ~108k
- **Python Files:** ~50
- **View Files (forms):** ~25+
- **Repository Files:** 9
- **JSON Localization:** 2

---

## Database Schema Summary

### SQLite (config.db)
- Application settings
- User preferences
- System configuration

### Session .mdb (SQLite variant)
**Tables:**
- Candidat - Student records
- Choix - Student choice preferences
- Concours - Examination info
- Filiere - Program/specialization
- Etablissement - Schools
- Matiere - Subjects
- Note - Grades
- Resultats - Computed results view

**Key Relationships:**
- Candidat → Etablissement (school)
- Choix → Candidat + Filiere
- Note → Candidat + Matiere
- Filiere → Etablissement

---

## Deployment

**Build Process (GitHub Actions):**
1. Checkout code
2. Setup Python 3.12
3. Install dependencies (requirements.txt + pyinstaller + pyodbc)
4. Create databases (scripts/create_dbs.py)
5. Build .exe (pyinstaller PhanBo-PFIEV.spec)
6. Package into ZIP
7. Create GitHub Release with .zip artifact

**User Deployment:**
1. Download PhanBo-PFIEV-win-x64.zip
2. Extract to desired location
3. Run PhanBo-PFIEV.exe (no Python installation needed)

---

## Known Patterns

1. **Singleton Pattern:** ConfigDB, SessionDB - ensure single DB connection
2. **Repository Pattern:** All CRUD via dedicated repo classes
3. **Service Layer:** AttributionService, ExcelService for business logic
4. **Localization Pattern:** Centralized L class for i18n
5. **Form-Based UI:** Each screen = separate Form class
6. **Data Classes:** Models use @dataclass with Optional fields

---

## Entry Point Flow

```
main.py
  ↓
  QApplication setup + matplotlib backend
  ↓
  Create config.db + template.mdb (if missing)
  ↓
  Initialize ConfigDB singleton
  ↓
  Show FrmLancement (language selector)
  ↓
  User creates/selects session
  ↓
  Initialize SessionDB → FrmSession
  ↓
  Navigate to FrmAccueil (main dashboard)
  ↓
  Access features: add candidates, import grades, run attribution, view results
  ↓
  Export results to Excel
```

---

## Notes for Developers

- **Threading:** No async/await. Main thread handles UI + DB.
- **Error Handling:** Try-except around DB operations. Silent fallback for non-critical failures.
- **Code Organization:** Models are pure dataclasses. Logic in repositories + services.
- **UI Testing:** Manual testing on Windows. No automated UI tests.
- **Localization:** Always use L.get() for displayed text. Never hardcode strings.
