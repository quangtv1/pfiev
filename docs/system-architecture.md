# System Architecture - PhanBo-PFIEV

**Version:** v0.1.3 | **Target:** Windows Desktop | **Last Updated:** 2026-03-23

---

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Windows Desktop App                       │
│                   PhanBo-PFIEV.exe                       │
└──────────────────────────┬──────────────────────────────────┘
                           │
            ┌──────────────┼──────────────┐
            │              │              │
    ┌───────▼────┐  ┌─────▼────┐  ┌─────▼────┐
    │   Views    │  │ Services │  │  Models  │
    │  (PySide6) │  │  Logic   │  │(Dataclass)
    └───────┬────┘  └─────┬────┘  └─────┬────┘
            │              │              │
            │    ┌─────────▼──────┐      │
            ├────┤ Repositories   │◄─────┘
            │    │ (CRUD + Query) │
            │    └─────────┬──────┘
            │              │
    ┌───────┴──────────────▼──────────────────┐
    │       Database Adapter                  │
    │  (Unified sqlite3 + pyodbc)            │
    └───────┬──────────────┬──────────────────┘
            │              │
    ┌───────▼──────┐  ┌───▼──────────────┐
    │  config.db   │  │ session.mdb      │
    │  (SQLite)    │  │ (SQLite/Access)  │
    │  Settings    │  │ Data             │
    └──────────────┘  └──────────────────┘
```

---

## 4-Layer Architecture

### Layer 1: Presentation (UI)
**Files:** `app/views/frm_*.py`
**Technology:** PySide6 (Qt6)

**Responsibilities:**
- Display data in forms/dialogs
- Capture user input (buttons, text fields, tables)
- Trigger business logic
- Update UI state

**Key Components:**
- `FrmLancement` - App launcher (language selector)
- `FrmSession` - Session switcher
- `FrmAccueil` - Main dashboard
- `FrmImport` - Excel import wizard
- `FrmResultats` - Results viewer
- `FrmStat*` - Charts & statistics (6+)

**Design Pattern:**
- Each view = separate class
- Inherits QWidget/QDialog
- Receives data, displays, sends events back

---

### Layer 2: Business Logic (Services)
**Files:** `app/services/`
**Technology:** Pure Python

**Responsibilities:**
- Core algorithms (attribution)
- Data transformation (Excel parsing)
- Complex operations (ranking, allocation)

**Services:**

#### AttributionService
```python
def run():
    # 1. Rank candidates by average
    # 2. For each candidate:
    #    - Iterate their choice list
    #    - Allocate to first filiere with capacity
    # 3. Mark choix_admis = True
```

**Algorithm:**
```
FOR EACH candidate (sorted by moyenne DESC):
    FOR EACH choice (sorted by order ASC):
        IF filiere has capacity:
            ASSIGN candidate → filiere
            BREAK
        END IF
    END FOR
END FOR
```

#### ExcelService
```python
def import_from_excel(file_path):
    # Parse .xlsx
    # Map columns
    # Validate rows
    # Return ImportRow[] with candidate + grades

def export_candidats(rows, file_path):
    # Create workbook
    # Format headers
    # Write candidate data
    # Save .xlsx

def export_resultats(rows, file_path):
    # Create workbook
    # Format headers
    # Write results + allocation
    # Save .xlsx
```

---

### Layer 3: Data Access (Repositories)
**Files:** `app/database/repositories/`
**Technology:** SQL (sqlite3/pyodbc)

**Pattern:** Repository per Entity

| Repository | Entity | Methods |
|------------|--------|---------|
| CandidatRepository | Candidat | get_all, get_by_id, add, update, delete, get_ranked_for_attribution |
| ChoixRepository | Choix | get_by_candidat, add, update, reset_admis, count_admis |
| FilierRepository | Filiere | get_all, get_by_id, add, update, set_nb_place |
| ResultatsRepository | (View) | get_results (complex JOIN) |
| NoteRepository | Note | add_grade, get_by_candidat, update |
| EtablissementRepository | Etablissement | get_all, add, update |
| MatiereRepository | Matiere | get_all, add, update |
| ConcoursRepository | Concours | get_current, update_settings |

**Responsibilities:**
- Map domain objects ↔ DB rows
- Execute SQL queries
- Handle commits/rollbacks
- Validate constraints

**Example: CandidatRepository.add()**
```python
def add(self, c: Candidat) -> int:
    cur = self._conn.execute(
        """INSERT INTO Candidat
           (Nom, Prenom, ...) VALUES (?, ?, ...)""",
        (c.nom, c.prenom, ...)
    )
    self._conn.commit()
    return cur.lastrowid  # Return new ID
```

---

### Layer 4: Data Storage (Persistence)
**Files:** `app/database/`
**Technology:** SQLite + optional pyodbc

#### ConfigDB (Singleton)
- **File:** `data/config.db`
- **Type:** SQLite
- **Purpose:** Application settings
- **Access:** Initialized once at startup

#### SessionDB (Singleton)
- **File:** `data/{session_name}.mdb`
- **Type:** SQLite (variant) or legacy Access
- **Purpose:** Current session data
- **Access:** Initialized when user selects session

#### DbAdapter
- **Class:** `DbConnection`
- **Purpose:** Unified wrapper for sqlite3 + pyodbc
- **Logic:**
  - Detect file extension (.mdb vs .db)
  - Check for Access driver availability
  - Choose sqlite3 or pyodbc accordingly

```python
class DbConnection:
    def __init__(self, path, force_sqlite=False):
        if path.endswith(".mdb") and has_access_driver():
            # Use pyodbc (real Access)
        else:
            # Use sqlite3 (SQLite)

    def execute(sql, params) -> Cursor:
        # Unified interface
        # SQL dialect translation (for Access)
```

---

## Database Schema

### Tables in Session .mdb

```
Candidat
├─ CandidatID (PK)
├─ Nom, NomIntermediaire, Prenom
├─ DateDeNaissance
├─ Sexe, CandidatStatut (I/E)
├─ Langue, EtabID (FK)
├─ CandidatMoyenne, CandidatClassement
└─ anonymat

Choix
├─ ChoixID (PK)
├─ CandidatID (FK)
├─ FiliereID (FK)
├─ ChoixOrdre (1-5)
├─ ChoixAdmis (bool)
└─ FiliereNbPlace (snapshot)

Filiere
├─ FiliereID (PK)
├─ FiliereNom
├─ NbPlace
├─ EtabFiliere
└─ EtabID (FK)

Établissement
├─ EtabID (PK)
├─ EtabNom
└─ EtabCode

Matiere
├─ MatiereID (PK)
├─ MatiereNom
└─ MatiereCode

Note
├─ NoteID (PK)
├─ CandidatID (FK)
├─ MatiereID (FK)
├─ NoteValeur
└─ Created_At

Concours
├─ ConcoursID (PK)
├─ Annee
├─ MoyenneMin
└─ Created_At

Resultats (View/Table)
├─ CandidatID, Nom, Prenom, ...
├─ CandidatMoyenne
├─ FiliereID, FiliereNom
├─ CandidatClassement
└─ ChoixAdmis
```

### Relationships
```
Candidat (1) ──── (M) Choix
Candidat (1) ──── (M) Note
Filiere (1) ──── (M) Choix
Etablissement (1) ──── (M) Candidat
Etablissement (1) ──── (M) Filiere
Matiere (1) ──── (M) Note
```

---

## Data Flow Diagrams

### 1. Import Flow
```
User selects Excel
        ↓
FrmImport.browse()
        ↓
ExcelService.import_from_excel(file_path)
        ├─ Parse .xlsx (openpyxl)
        ├─ Map columns
        ├─ Validate rows
        └─ Return ImportRow[]
        ↓
FrmImport displays preview
        ↓
User confirms
        ↓
CandidatRepository.add() × N
        ↓
NoteRepository.add_grades() × N
        ↓
Session .mdb updated ✓
```

### 2. Attribution Flow
```
User clicks "Run Attribution"
        ↓
Validation checks
├─ All candidates have grades?
├─ All choices set?
└─ Filiere capacities defined?
        ↓
AttributionService.run()
├─ CandidatRepository.compute_averages_and_ranking()
├─ ChoixRepository.reset_admis()
├─ FOR each candidate (ranked):
│    FOR each choice (ordered):
│        IF filiere has capacity:
│            UPDATE Choix.ChoixAdmis = 1
│            BREAK
        ↓
ResultatsRepository.get_results()
        ↓
FrmResultats displays allocation
        ↓
ExcelService.export_resultats()
        └─ Save Excel with results ✓
```

### 3. Session Lifecycle
```
App startup
        ↓
FrmLancement shown
        ↓
User selects language: L.set_language("vi")
        ↓
User chooses session (create or open)
        ↓
SessionDB.initialize(path)
└─ Opens session.mdb
├─ Validates schema
└─ Ready for data access
        ↓
FrmSession shown
├─ Load candidate count
├─ Show last updated time
├─ Load attribution status
        ↓
User navigates FrmAccueil (main menu)
        ↓
User works: add candidates, import, run attribution, view results
        ↓
User exits
        ↓
SessionDB.close()
└─ Commit pending transactions
```

---

## State Management

### AppState (Global Singleton)
```python
class AppState:
    language: str = "vi"              # Current language
    is_first_open: bool = False       # First-time user?
    session_in_progress: bool = False # Active session?
    max_choices: int = 5              # Max filiere choices
```

**Usage:**
```python
AppState.language = "fr"
L.set_language(AppState.language)  # Update localization
```

### Localization (L Class)
```python
class L:
    _strings: dict = {}

    @classmethod
    def set_language(cls, lang: str):
        cls._strings = json.load(f"resources/strings_{lang}.json")

    @classmethod
    def get(cls, key: str) -> str:
        return cls._strings.get(key, key)
```

**Strings Format:**
```json
{
    "btn_import": "Import Excel",
    "btn_export": "Export Results",
    "msg_success": "Operation completed successfully!"
}
```

---

## Module Dependencies

```
main.py
├─ app.state (AppState)
├─ app.localization (L)
├─ app.database.config_db (ConfigDB)
└─ app.views.frm_lancement (FrmLancement)

app/views/frm_*.py
├─ app.state
├─ app.localization
├─ app.database.config_db / session_db
├─ app.database.repositories.*
├─ app.services.attribution_service / excel_service
└─ app.models.*

app/services/attribution_service.py
├─ app.database.session_db
├─ app.database.repositories.candidat_repo
└─ app.database.repositories.choix_repo

app/database/repositories/*.py
├─ app.database.session_db
├─ app.models.*
└─ (indirect: app.database.db_adapter)

app/database/session_db.py
├─ app.database.db_adapter (DbConnection)
└─ Singleton manages .mdb connection
```

---

## Error Handling Strategy

### Levels

| Level | Responsibility | Example |
|-------|-----------------|---------|
| **View** | UI feedback | Show error dialog, disable button |
| **Service** | Business logic validation | Raise ValueError if invalid state |
| **Repository** | DB constraints | Catch sqlite3.IntegrityError |
| **Adapter** | Connection/dialect | Handle pyodbc vs sqlite3 differences |

### Pattern
```python
try:
    result = repository.add(model)
except sqlite3.IntegrityError as e:
    logger.error(f"DB constraint violated: {e}")
    show_error_dialog("Duplicate entry detected")
except Exception as e:
    logger.error(f"Unexpected error: {e}")
    show_error_dialog("Operation failed. Please contact support.")
```

---

## Performance Considerations

### Optimizations Implemented
1. **Database Indexing** - PK/FK on all tables
2. **Connection Pooling** - Singleton connections (ConfigDB, SessionDB)
3. **Query Optimization** - JOINs in repositories, not in Python
4. **Lazy Loading** - Load only when needed (e.g., results on export)

### Bottlenecks & Solutions

| Bottleneck | Cause | Solution (v0.2+) |
|-----------|-------|-----------------|
| Large dataset load | All-in-memory list | Pagination, lazy load |
| Attribution for 500+ HS | O(n×m) iteration | Batch SQL update, indexed lookup |
| Excel export (1000 rows) | openpyxl processing | Streaming writer, chunk export |
| Chart generation | matplotlib rendering | Async generation, caching |

---

## Security Architecture

### Current (v0.1.3)
- ✅ No authentication (local desktop app)
- ✅ Database path validation
- ✅ SQL injection prevention (parameterized queries)
- ✅ No sensitive data in logs

### Planned (v0.4+)
- [ ] Encrypt .mdb files at rest (AES-256)
- [ ] Local authentication (password)
- [ ] Audit logging (who changed what)
- [ ] Role-based access control (admin/teacher/viewer)

---

## Deployment Architecture

### Build Pipeline
```
Developer pushes tag (git push --tags)
        ↓
GitHub Actions triggered
        ↓
Windows runner setup (Python 3.12)
        ↓
Install dependencies
├─ pip install -r requirements.txt
├─ pip install pyinstaller pyodbc
        ↓
Create databases
└─ python scripts/create_dbs.py
        ↓
Build .exe
└─ pyinstaller PhanBo-PFIEV.spec
        ↓
Package ZIP
└─ Compress-Archive dist/PhanBo-PFIEV
        ↓
Create GitHub Release
└─ Upload .zip artifact
        ↓
Notify users (Release notes)
```

### User Distribution
```
User downloads PhanBo-PFIEV-win-x64.zip
        ↓
Extract to C:\Program Files\PhanBo-PFIEV
        ↓
Run PhanBo-PFIEV.exe
        ├─ Auto-creates data/ folder
        ├─ Auto-creates config.db
        ├─ Auto-creates template.mdb
        ↓
App ready to use (no Python needed) ✓
```

---

## Scalability & Future

### Current Limits (v0.1.3)
- Max ~200-300 students/session (UI responsiveness)
- Single-threaded (no async)
- In-memory data models
- SQLite (local file)

### Scaling Path (v1.0+)

| Phase | Change | Impact |
|-------|--------|--------|
| **v0.2** | Pagination, lazy loading | Support 500+ HS |
| **v0.3** | Async operations, caching | Improve responsiveness |
| **v0.4** | Database migration layer | Support PostgreSQL |
| **v1.0** | API + REST endpoints | Multi-client support |

---

## Component Interaction Example

### "Import & Rank" Workflow

```
1. User clicks FrmImport.btn_browse
   └─ Opens file dialog

2. FrmImport receives file path
   └─ Calls ExcelService.import_from_excel(path)
      ├─ Parses .xlsx → ImportRow[]
      └─ Returns list with candidate + grade data

3. FrmImport displays preview
   └─ User reviews rows

4. User clicks "Confirm Import"
   └─ For each ImportRow:
      ├─ CandidatRepository.add(candidat)
      ├─ NoteRepository.add_grade(candidat_id, matiere_id, valeur)

5. After import, FrmImport triggers ranking
   └─ CandidatRepository.compute_averages_and_ranking()
      ├─ UPDATE Candidat.CandidatMoyenne = AVG(Note)
      └─ UPDATE Candidat.CandidatClassement = RANK()

6. FrmImport shows success
   └─ "100 students imported, 50 ranked above threshold"

7. User navigates to FrmResultats
   └─ ResultatsRepository.get_results()
      └─ SELECT * FROM (Candidat JOIN Choix JOIN Filiere)
         ORDER BY CandidatClassement ASC
   └─ Display in table with allocation status
```

---

## Diagram: Request Flow

```
User Action
    ↓
View captures event
    ↓
Call Service / Repository
    ↓
Service processes logic
    ↓
Repository executes SQL
    ↓
DbAdapter runs query
    ↓
SQLite/pyodbc returns result
    ↓
Repository returns model
    ↓
Service returns processed data
    ↓
View updates UI
    ↓
User sees result ✓
```
