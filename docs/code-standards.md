# Code Standards & Conventions - PhanBo-PFIEV

**Version:** v0.1.3 | **Last Updated:** 2026-03-23

---

## General Principles

### YAGNI (You Aren't Gonna Need It)
- Code for current requirements only
- Avoid premature optimization
- Don't add "nice-to-have" features upfront

### KISS (Keep It Simple, Stupid)
- Prefer clarity over cleverness
- Simple algorithms > complex optimizations
- Readable code > compact code

### DRY (Don't Repeat Yourself)
- Extract common logic into functions/services
- Use inheritance for shared behavior
- Share SQL queries via repositories

---

## Python Style Guide

### Code Structure

**File Naming:**
```python
# Modules (lowercase with underscores)
candidat_repo.py          # repository
excel_service.py          # service
frm_ajouter_candidat.py   # view form
db_adapter.py             # infrastructure

# Classes (PascalCase)
class CandidatRepository:
    pass

class ExcelService:
    pass

class FrmAjouterCandidat(QDialog):
    pass

# Constants (UPPERCASE with underscores)
MAX_CHOICES = 5
DEFAULT_LANGUAGE = "vi"
DB_TIMEOUT = 30
```

**Module Imports:**
```python
# 1. Standard library
import sys
import json
from pathlib import Path
from typing import Optional

# 2. Third-party
from PySide6.QtWidgets import QDialog, QTableWidget
from openpyxl import Workbook
import matplotlib.pyplot as plt

# 3. Local application
from app.models.candidat import Candidat
from app.database.repositories.candidat_repo import CandidatRepository
from app.localization import L
```

---

### Naming Conventions

| Concept | Style | Example |
|---------|-------|---------|
| **Class** | PascalCase | `CandidatRepository`, `FrmImport` |
| **Function/Method** | snake_case | `get_all()`, `add_candidat()` |
| **Constant** | UPPER_SNAKE | `MAX_CHOICES`, `DEFAULT_LANGUAGE` |
| **Variable** | snake_case | `candidat_id`, `avg_score` |
| **Private method** | _snake_case | `_validate_email()` |
| **Database column** | PascalCase | `CandidatID`, `FiliereNom` |
| **UI element** | Snake_case with prefix | `btn_import`, `txt_search`, `tbl_results` |

**Database Column Mapping:**
```python
# Model uses snake_case
@dataclass
class Candidat:
    candidat_id: int
    nom: str

# Repository maps to DB columns (PascalCase)
def _row_to_model(self, row) -> Candidat:
    return Candidat(
        candidat_id=row['CandidatID'],
        nom=row['Nom']
    )
```

---

### Type Hints

**Always use type hints:**
```python
# ✅ Good
def add_candidat(self, c: Candidat) -> int:
    return cur.lastrowid

def get_by_id(self, cid: int) -> Optional[Candidat]:
    return result if result else None

# ❌ Bad
def add_candidat(self, c):
    return cur.lastrowid
```

**Collections:**
```python
# ✅ Good
def get_all(self) -> list[Candidat]:
    return [self._row_to_model(r) for r in rows]

def get_choices(self) -> dict[int, list[Choix]]:
    return {candidat_id: choices}

# ❌ Bad
def get_all(self):
    return rows
```

**Optional fields:**
```python
# ✅ Good
candidat_moyenne: Optional[float] = None
date_de_naissance: Optional[str] = None

# ❌ Bad
candidat_moyenne = None  # Unclear type
```

---

### Docstrings

**Function/Method docstrings:**
```python
def compute_averages(self) -> None:
    """Compute and update average scores for all candidates.

    Calculates average grade from all Note records per Candidat.
    Updates CandidatMoyenne in database. Commits transaction.

    Raises:
        RuntimeError: If database connection not initialized.

    Example:
        >>> repo = CandidatRepository()
        >>> repo.compute_averages()
    """
    pass
```

**Class docstrings:**
```python
class AttributionService:
    """Allocate candidates to filieres based on ranking and preferences.

    Algorithm:
    1. Rank candidates by average score (descending)
    2. For each candidate, try their ordered choices
    3. Assign to first filiere with available capacity
    4. If no capacity: candidate remains unassigned

    Port of VB6 AttributionSpecialite() logic.
    """
    pass
```

**Inline comments:**
```python
# ✅ Good - explains WHY, not WHAT
if admis_count < nb_place:
    # Filiere has capacity, assign immediately to first choice
    conn.execute("UPDATE Choix SET ChoixAdmis=1 WHERE ChoixID=?", (choix_id,))

# ❌ Bad - obvious from code
if admis_count < nb_place:
    # Increment admis count
    conn.execute(...)
```

---

## Architecture Patterns

### Model Layer (Models)

**Use dataclasses:**
```python
from dataclasses import dataclass
from typing import Optional

@dataclass
class Candidat:
    candidat_id: int = 0
    nom: str = ""
    prenom: str = ""
    date_de_naissance: Optional[str] = None
    candidat_moyenne: Optional[float] = None
```

**Rules:**
- Dataclasses only for data storage
- No methods (except __post_init__)
- All fields have defaults
- Use @dataclass decorator

---

### Repository Layer

**CRUD Operations:**
```python
class CandidatRepository:
    def __init__(self):
        self._conn = SessionDB.conn()

    # Create
    def add(self, c: Candidat) -> int:
        cur = self._conn.execute(
            "INSERT INTO Candidat (...) VALUES (...)",
            (...)
        )
        self._conn.commit()
        return cur.lastrowid

    # Read
    def get_all(self) -> list[Candidat]:
        rows = self._conn.execute("SELECT * FROM Candidat").fetchall()
        return [self._row_to_model(r) for r in rows]

    def get_by_id(self, cid: int) -> Optional[Candidat]:
        row = self._conn.execute(
            "SELECT * FROM Candidat WHERE CandidatID=?", (cid,)
        ).fetchone()
        return self._row_to_model(row) if row else None

    # Update
    def update(self, c: Candidat) -> None:
        self._conn.execute(
            "UPDATE Candidat SET Nom=?, ... WHERE CandidatID=?",
            (c.nom, ..., c.candidat_id)
        )
        self._conn.commit()

    # Delete
    def delete(self, cid: int) -> None:
        self._conn.execute("DELETE FROM Candidat WHERE CandidatID=?", (cid,))
        self._conn.commit()

    # Helper
    def _row_to_model(self, row) -> Optional[Candidat]:
        if row is None:
            return None
        return Candidat(
            candidat_id=row['CandidatID'],
            nom=row['Nom'],
            ...
        )
```

**Rules:**
- One repository per entity
- Constructor takes connection from singleton (SessionDB)
- All SQL via execute() + parameterized queries (prevent SQL injection)
- Always commit after write operations
- Return models or None, not raw rows

---

### Service Layer

**Business Logic:**
```python
class AttributionService:
    @staticmethod
    def run() -> None:
        """Run allocation algorithm."""
        conn = SessionDB.conn()

        # Get candidates ranked by score
        candidat_repo = CandidatRepository()
        choix_repo = ChoixRepository()
        ranked_ids = candidat_repo.get_ranked_for_attribution()

        # For each candidate, allocate to best available choice
        for candidat_id in ranked_ids:
            choices = conn.execute(
                "SELECT ... WHERE CandidatID=? ORDER BY ChoixOrdre",
                (candidat_id,)
            ).fetchall()

            for choice in choices:
                if choix_repo.count_admis(choice['FiliereID']) < choice['NbPlace']:
                    conn.execute(
                        "UPDATE Choix SET ChoixAdmis=1 WHERE ChoixID=?",
                        (choice['ChoixID'],)
                    )
                    conn.commit()
                    break
```

**Rules:**
- Static methods for algorithms
- Use repositories for data access
- No direct SQL queries (delegate to repos)
- Validate inputs
- Raise exceptions on errors
- Return result or None

---

### View Layer (PySide6)

**Form Template:**
```python
from PySide6.QtWidgets import (
    QDialog, QVBoxLayout, QHBoxLayout,
    QPushButton, QTableWidget, QLineEdit, QLabel
)
from app.localization import L
from app.models.candidat import Candidat
from app.database.repositories.candidat_repo import CandidatRepository

class FrmAjouterCandidat(QDialog):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.setWindowTitle(L.get("frm_ajouter_candidat_title"))
        self._setup_ui()
        self._connect_signals()

    def _setup_ui(self):
        """Build UI components."""
        layout = QVBoxLayout()

        self.txt_nom = QLineEdit()
        self.txt_nom.setPlaceholderText(L.get("placeholder_nom"))

        self.btn_save = QPushButton(L.get("btn_save"))

        layout.addWidget(QLabel(L.get("label_nom")))
        layout.addWidget(self.txt_nom)
        layout.addWidget(self.btn_save)

        self.setLayout(layout)

    def _connect_signals(self):
        """Connect button/input signals to slots."""
        self.btn_save.clicked.connect(self._on_save_clicked)

    def _on_save_clicked(self):
        """Handle save button click."""
        try:
            candidat = Candidat(
                nom=self.txt_nom.text()
            )
            repo = CandidatRepository()
            new_id = repo.add(candidat)
            self.accept()
        except Exception as e:
            self._show_error(f"Lỗi: {str(e)}")

    def _show_error(self, msg: str):
        """Display error dialog."""
        from PySide6.QtWidgets import QMessageBox
        QMessageBox.critical(self, L.get("error"), msg)
```

**Rules:**
- Separate _setup_ui() and _connect_signals()
- Use L.get() for all displayed text
- Methods prefixed with _ are private
- Use try-except for user actions
- Show error dialogs for failures
- Delegate business logic to services/repositories

---

## Database Standards

### SQL Conventions

**Parameterized queries (prevent SQL injection):**
```python
# ✅ Good
cursor.execute("SELECT * FROM Candidat WHERE CandidatID=?", (cid,))

# ❌ Bad - SQL injection risk
cursor.execute(f"SELECT * FROM Candidat WHERE CandidatID={cid}")
```

**Column references (use exact DB names):**
```python
# ✅ Good - matches schema
cursor.execute(
    "SELECT CandidatID, Nom, Prenom FROM Candidat WHERE CandidatID=?",
    (cid,)
)

# ❌ Bad - wrong column name
cursor.execute("SELECT candidat_id, nom FROM Candidat WHERE id=?", (cid,))
```

**JOINs in repository methods:**
```python
# ✅ Good - query returns complete data
def get_session_view(self) -> list[dict]:
    """Get candidates with school names."""
    sql = """
        SELECT c.CandidatID, c.Nom, c.Prenom, e.EtabNom
        FROM Candidat c
        LEFT JOIN Etablissement e ON c.EtabID = e.EtabID
        ORDER BY c.CandidatID
    """
    return [dict(row) for row in self._conn.execute(sql).fetchall()]

# ❌ Bad - N+1 query problem
def get_all(self) -> list:
    candidates = self._conn.execute("SELECT * FROM Candidat").fetchall()
    for c in candidates:
        etab = self._conn.execute(
            "SELECT EtabNom FROM Etablissement WHERE EtabID=?",
            (c['EtabID'],)
        ).fetchone()
```

---

### Database Transactions

**Multi-step operations:**
```python
try:
    # Step 1: Insert candidate
    cur = self._conn.execute(
        "INSERT INTO Candidat (...) VALUES (...)",
        (...)
    )
    candidat_id = cur.lastrowid

    # Step 2: Insert grades
    for matiere_id, value in grades.items():
        self._conn.execute(
            "INSERT INTO Note (CandidatID, MatiereID, NoteValeur) VALUES (?, ?, ?)",
            (candidat_id, matiere_id, value)
        )

    # All succeeded - commit
    self._conn.commit()
    return candidat_id

except Exception as e:
    # Implicit rollback on exception
    raise
```

---

## Error Handling

### Exception Strategy

**Raise exceptions for errors:**
```python
# ✅ Good
def add(self, c: Candidat) -> int:
    if not c.nom:
        raise ValueError("Candidate name cannot be empty")
    try:
        cur = self._conn.execute(...)
        return cur.lastrowid
    except sqlite3.IntegrityError:
        raise RuntimeError(f"Duplicate candidate: {c.nom}")

# ❌ Bad - silent failures
def add(self, c: Candidat) -> int:
    try:
        return self._conn.execute(...).lastrowid
    except:
        return -1  # Unclear what went wrong
```

**Handle in view layer:**
```python
def _on_import_clicked(self):
    try:
        count = self.service.import_candidates(self.file_path)
        QMessageBox.information(self, "Success", f"{count} candidates imported")
    except ValueError as e:
        QMessageBox.warning(self, "Validation Error", str(e))
    except Exception as e:
        QMessageBox.critical(self, "Error", f"Import failed: {str(e)}")
```

---

## Localization Standards

**All user-visible strings in L class:**

```python
# ✅ Good
self.setWindowTitle(L.get("frm_ajouter_candidat_title"))
self.btn_save.setText(L.get("btn_save"))

msg = L.fmt("msg_imported", count)  # "msg_imported": "Imported {0} candidates"

# ❌ Bad - hardcoded strings
self.setWindowTitle("Ajouter Candidat")
self.btn_save.setText("Save")
```

**JSON structure (resources/strings_vi.json):**
```json
{
    "frm_ajouter_candidat_title": "Thêm Học Sinh",
    "btn_save": "Lưu",
    "btn_cancel": "Hủy",
    "btn_import": "Import Excel",
    "placeholder_nom": "Nhập họ tên...",
    "label_nom": "Họ tên:",
    "msg_imported": "Đã import {0} học sinh",
    "error_empty_field": "Vui lòng điền tất cả các trường",
    "error_invalid_date": "Ngày sinh không hợp lệ"
}
```

---

## Testing Standards

### Unit Tests (Future v0.2+)

**Test file naming:**
```
test_candidat_repo.py
test_attribution_service.py
test_excel_service.py
```

**Test structure:**
```python
import unittest
from app.models.candidat import Candidat
from app.database.repositories.candidat_repo import CandidatRepository

class TestCandidatRepository(unittest.TestCase):
    def setUp(self):
        """Initialize test database."""
        self.repo = CandidatRepository()

    def test_add_candidat(self):
        """Test adding a new candidate."""
        c = Candidat(nom="Nguyen", prenom="Van A")
        cid = self.repo.add(c)
        self.assertGreater(cid, 0)

    def test_get_by_id(self):
        """Test retrieving a candidate by ID."""
        c = Candidat(nom="Tran", prenom="Thi B")
        cid = self.repo.add(c)
        retrieved = self.repo.get_by_id(cid)
        self.assertEqual(retrieved.nom, "Tran")
```

---

## Performance Guidelines

### Optimization Rules

1. **Index frequently queried columns:**
   ```sql
   CREATE INDEX idx_candidat_etab ON Candidat(EtabID);
   CREATE INDEX idx_choix_candidat ON Choix(CandidatID);
   ```

2. **Use LIMIT for large result sets:**
   ```python
   def get_recent_candidates(self, limit: int = 100) -> list[Candidat]:
       sql = "SELECT * FROM Candidat ORDER BY CandidatID DESC LIMIT ?"
       rows = self._conn.execute(sql, (limit,)).fetchall()
       return [self._row_to_model(r) for r in rows]
   ```

3. **Batch inserts instead of loop:**
   ```python
   # ✅ Good - single transaction
   cursor.executemany(
       "INSERT INTO Note (CandidatID, MatiereID, NoteValeur) VALUES (?, ?, ?)",
       grade_rows
   )

   # ❌ Bad - N transactions
   for grade in grade_rows:
       cursor.execute("INSERT INTO Note ...", grade)
   ```

4. **Cache read-only data:**
   ```python
   class FilierRepository:
       _cache = None

       def get_all(self) -> list:
           if self._cache is None:
               rows = self._conn.execute("SELECT * FROM Filiere").fetchall()
               self._cache = [self._row_to_model(r) for r in rows]
           return self._cache
   ```

---

## Commit Message Standards

**Format:**
```
<type>: <subject>

<body>
```

**Type:**
- `feat:` New feature
- `fix:` Bug fix
- `refactor:` Code restructuring (no logic change)
- `docs:` Documentation update
- `test:` Test addition/update
- `chore:` Build, deps, config

**Examples:**
```
feat: add candidat import from Excel

Users can now bulk-import candidate data from .xlsx files.
Validates columns, prevents duplicates, auto-computes averages.

fix: AttributionService respects max_choices limit

Previously ignored choix_ordre > max_choices. Now skips invalid choices.

docs: update system architecture diagram
```

---

## Code Review Checklist

Before committing:

- [ ] Type hints on all functions/methods
- [ ] Docstrings on classes/public methods
- [ ] No hardcoded strings (use L.get())
- [ ] Parameterized SQL queries
- [ ] Error handling (try-except) for user actions
- [ ] No unused imports
- [ ] Consistent naming (snake_case/PascalCase)
- [ ] Database commits after mutations
- [ ] Tests pass (if applicable)

---

## File Size Limits

- **Python files:** < 200 lines (split if larger)
- **Forms:** < 300 lines (consider extracting dialogs)
- **Repositories:** < 150 lines (split complex queries)
- **Services:** < 100 lines (keep focused)

If exceeded, refactor into smaller, focused modules.
