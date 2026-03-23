# Deployment Guide - OrientationPFIEV-Python

**Version:** v0.1.3 | **Target:** Windows 10+ | **Last Updated:** 2026-03-23

---

## Quick Start (End Users)

### Installation

1. **Download**
   - Go to: https://github.com/quangtv1/pfiev/releases
   - Download `OrientationPFIEV-Python-win-x64.zip` (latest version)

2. **Extract**
   - Right-click → Extract All
   - Unzip to: `C:\Program Files\OrientationPFIEV` (recommended)
   - Or any location (Desktop, Documents, etc.)

3. **Run**
   - Double-click `OrientationPFIEV.exe`
   - Select language (Tiếng Việt / Français)
   - Create or open a session
   - Start using!

### Requirements
- **OS:** Windows 10 or later (64-bit)
- **RAM:** 512 MB minimum
- **Storage:** 100 MB free space
- **No Python installation needed!** (bundled in .exe)

### Troubleshooting

**"OrientationPFIEV.exe not found"**
- Ensure you extracted the entire folder, not just the .exe
- Check that `dist/OrientationPFIEV/` folder is complete

**"VCRUNTIME140.dll missing"**
- Download Visual C++ Redistributable from Microsoft
- Install: https://support.microsoft.com/en-us/help/2977003/

**"Access driver not available"**
- Some legacy .mdb files may not open
- Fallback: Use SQLite session files (.mdb as SQLite)
- Install ODBC driver: https://www.microsoft.com/en-us/download/details.aspx?id=13255

**App crashes on startup**
- Check for antivirus blocking (whitelist OrientationPFIEV.exe)
- Verify write permissions in installation folder
- Check `data/` folder exists and is writable

---

## Developer Setup

### Prerequisites

- **Python:** 3.12+ (Download from python.org)
- **Git:** For cloning the repository
- **pip:** Python package manager (included with Python)

### Clone & Setup

```bash
# Clone repository
git clone https://github.com/quangtv1/pfiev.git
cd OrientationPFIEV-Python

# Create virtual environment
python -m venv venv

# Activate virtual environment
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# Install dependencies
pip install -r requirements.txt

# Create databases (if not already present)
python scripts/create_dbs.py

# Run application
python main.py
```

### Development Dependencies

```bash
# Add these for development
pip install pytest pytest-cov           # Testing
pip install black isort flake8          # Code quality
pip install pyinstaller                 # Building .exe
```

---

## Building from Source

### Step 1: Install PyInstaller

```bash
pip install pyinstaller pyinstaller-hooks-contrib pyodbc
```

### Step 2: Create Databases (if missing)

```bash
python scripts/create_dbs.py
```

This creates:
- `data/config.db` - SQLite configuration database
- `data/template.mdb` - Template for session files

### Step 3: Build .exe with PyInstaller

```bash
pyinstaller OrientationPFIEV.spec
```

**Output:** `dist/OrientationPFIEV/OrientationPFIEV.exe`

### Step 4: Test Executable

```bash
# Run the built .exe
dist/OrientationPFIEV/OrientationPFIEV.exe
```

### Step 5: Package for Distribution

```bash
# Windows PowerShell
Compress-Archive -Path dist/OrientationPFIEV -DestinationPath OrientationPFIEV-Python-win-x64.zip

# macOS/Linux
zip -r OrientationPFIEV-Python-win-x64.zip dist/OrientationPFIEV/
```

### Build Configuration (OrientationPFIEV.spec)

**Key settings:**
```python
# Include data files
datas=[
    ('resources', 'resources'),    # Localization + icons
    ('data', 'data'),               # Database templates
] + collect_data_files('matplotlib')

# Hidden imports (bundled with .exe)
hiddenimports=[
    'PySide6.QtCore',
    'PySide6.QtWidgets',
    'PySide6.QtGui',
    'matplotlib.backends.backend_qtagg',
    'openpyxl',
    'pyodbc',
]

# Excluded (reduce .exe size)
excludes=[
    'tkinter', 'PyQt5', 'PyQt6',
    'PySide6.QtWebEngine',
    'PySide6.QtQuick',
    # ... (25+ others omitted)
]

# Icon
icon='resources/iconPFEIV.ico'
```

---

## GitHub Actions CI/CD

### Workflow: Automated Build & Release

**File:** `.github/workflows/build.yml`

**Triggers:**
- Push to `main` branch
- Push tag matching `v*` (e.g., `v0.1.3`)
- Manual trigger (`workflow_dispatch`)

### Build Process

```yaml
1. Checkout code
2. Setup Python 3.12
3. Install dependencies
   - requirements.txt
   - pyinstaller
   - pyodbc
4. Create databases
   - python scripts/create_dbs.py
5. Build .exe
   - pyinstaller OrientationPFIEV.spec
6. Package ZIP
   - Compress-Archive dist/OrientationPFIEV
7. Upload artifact
   - Save for 30 days
8. Create GitHub Release
   - Only if tag pushed (vX.Y.Z)
   - Attach .zip file
   - Post release notes
```

### Creating a Release

**Step 1: Bump version in code**

Update `project-overview-pdr.md` and any version files:

```markdown
**Current Version:** v0.2.0
```

**Step 2: Commit & tag**

```bash
git add .
git commit -m "Release v0.2.0"
git tag v0.2.0
git push origin main
git push origin v0.2.0
```

**Step 3: GitHub Actions runs automatically**

- Go to: https://github.com/quangtv1/pfiev/actions
- Watch build progress
- Artifact appears in ~5 minutes

**Step 4: Release appears on GitHub**

- Go to: https://github.com/quangtv1/pfiev/releases
- Latest build is auto-published
- Download `.zip` from release page

---

## Database Management

### config.db (Settings)

**Location:** `data/config.db`
**Type:** SQLite
**Purpose:** Application settings

**Tables:**
- AppSettings (key-value pairs)
- Users (if auth added later)

**Backup:**
```bash
# Manual backup
cp data/config.db data/config.db.backup

# Restore
cp data/config.db.backup data/config.db
```

**Reset (clear all settings):**
```bash
rm data/config.db
python scripts/create_dbs.py
```

### Session .mdb Files

**Location:** `data/{session_name}.mdb`
**Type:** SQLite (variant) or legacy Access
**Purpose:** Candidate data, grades, choices, results

**Auto-backup:**
- Before running attribution
- Copy to: `data/{session_name}_backup.mdb`

**Manual backup:**
```bash
# Backup current session
cp "data/session_2026_03.mdb" "data/session_2026_03_backup_2026-03-23.mdb"
```

**Restore from backup:**
```bash
cp "data/session_2026_03_backup.mdb" "data/session_2026_03.mdb"
```

### Schema Migration (Future)

**If schema changes (v0.2+):**

1. Create migration script: `scripts/migrate_vX_Y.py`
2. Backup all .mdb files
3. Run migration on each session file
4. Verify data integrity

---

## Performance Optimization

### For Large Datasets (500+ students)

1. **Indexing:**
   ```sql
   CREATE INDEX idx_candidat_moyenne ON Candidat(CandidatMoyenne DESC);
   CREATE INDEX idx_choix_candidat ON Choix(CandidatID);
   CREATE INDEX idx_choix_filiere ON Choix(FiliereID);
   ```

2. **Query optimization:**
   - Use JOINs in SQL (not Python loops)
   - Limit result sets with LIMIT clause
   - Cache read-only data (Filiere, Matiere)

3. **Attribution speed:**
   - Update in batch (executemany)
   - Use transactions for multi-step operations
   - Avoid N+1 queries

4. **UI responsiveness:**
   - Pagination for large tables (load 100 rows at a time)
   - Async operations for slow tasks (v0.2+)
   - Progress bars for long operations

### Profiling

```python
import cProfile
import pstats

# Profile attribution
profiler = cProfile.Profile()
profiler.enable()

AttributionService.run()

profiler.disable()
stats = pstats.Stats(profiler)
stats.sort_stats('cumulative')
stats.print_stats(20)  # Top 20 functions
```

---

## Security Checklist

### Pre-Release

- [ ] No hardcoded API keys or passwords in code
- [ ] SQL queries are parameterized (prevent injection)
- [ ] No sensitive data in error messages
- [ ] Exception handling hides implementation details
- [ ] .gitignore excludes .env, .mdb files
- [ ] No debug mode in production build

### User Data Protection

- [ ] Backup .mdb files before operations
- [ ] Validate file integrity after import
- [ ] Encrypt sensitive data at rest (v0.4+)
- [ ] Log all mutations for audit (v0.4+)

### Deployment

- [ ] Whitelist .exe in antivirus before distribution
- [ ] Sign executable (code signing certificate, v1.0+)
- [ ] Host releases on GitHub (immutable archive)
- [ ] Verify checksums for downloaded files (v1.0+)

---

## Troubleshooting Build Issues

### "PyInstaller: command not found"

```bash
pip install pyinstaller
```

### "openpyxl not found during build"

```bash
pip install -r requirements.txt
pip install pyinstaller pyinstaller-hooks-contrib
```

### "matplotlib backend error"

- Ensure backend is set before import: `matplotlib.use('QtAgg')`
- Hidden import already included in `.spec`

### ".exe works, but app won't start"

**Causes:**
1. Missing `resources/` folder
2. Missing `data/` folder or templates
3. Icon file not found
4. Config database corrupted

**Fix:**
```bash
# Verify dist/ structure
dist/OrientationPFIEV/
├── OrientationPFIEV.exe
├── resources/
│   ├── strings_vi.json
│   ├── strings_fr.json
│   └── iconPFEIV.ico
├── data/
│   ├── config.db
│   └── template.mdb
└── ... (PySide6, matplotlib libs, etc.)
```

### "AttributeError: module has no attribute"

- Hidden import missing in `.spec`
- Add to `hiddenimports` list
- Rebuild with `pyinstaller OrientationPFIEV.spec`

---

## Version Management

### Semantic Versioning

Format: `MAJOR.MINOR.PATCH` (e.g., `v0.1.3`)

| Part | Changes | Example |
|------|---------|---------|
| MAJOR | Breaking changes | v1.0.0 (API change, incompatible) |
| MINOR | New features | v0.2.0 (new import wizard) |
| PATCH | Bug fixes | v0.1.3 (fix UI crash) |

### Version Tracking

**Update in these files after release:**
1. `docs/project-overview-pdr.md` - Update version line
2. GitHub Releases - Create release with notes
3. README.md (if exists) - Update download link

---

## Maintenance Schedule

### Monthly
- [ ] Check for bug reports on GitHub Issues
- [ ] Review code quality metrics
- [ ] Update dependency versions (if needed)

### Quarterly
- [ ] Review user feedback
- [ ] Update documentation
- [ ] Plan next feature release

### Annually
- [ ] Major version planning
- [ ] Security audit
- [ ] Performance benchmarking

---

## Rollback Procedure

**If latest version has critical bug:**

```bash
# Step 1: Tag as buggy
git tag -d v0.2.0
git tag v0.2.0-broken

# Step 2: Revert to previous version
git checkout v0.1.3
git tag v0.2.1
git push origin --tags

# Step 3: Users download v0.2.1 instead
# Old v0.2.0 available but marked as broken
```

---

## Environment Variables

**No sensitive data in .exe!**

If needed (v0.4+), use:
- Config file: `data/config.db` (encrypted)
- Environment variables: Windows Registry (encrypted)
- Never hardcode in Python files

---

## Monitoring & Logging

### User Error Reporting

**If crash happens:**

```python
# Log to user's home directory
import logging
log_path = Path.home() / ".OrientationPFIEV" / "app.log"
logging.basicConfig(filename=log_path, level=logging.ERROR)
```

**User can then:**
1. Report bug on GitHub Issues
2. Attach log file for debugging

---

## Support Resources

| Resource | URL |
|----------|-----|
| **Releases** | https://github.com/quangtv1/pfiev/releases |
| **Issues** | https://github.com/quangtv1/pfiev/issues |
| **Documentation** | ./docs/ (this folder) |
| **Source Code** | https://github.com/quangtv1/pfiev |

---

## Release Checklist

Before tagging a release:

- [ ] All tests pass
- [ ] Code reviewed
- [ ] Documentation updated
- [ ] Version bumped in docs
- [ ] Git log is clean
- [ ] Build artifacts tested on Windows
- [ ] Release notes written
- [ ] Tag created and pushed

```bash
# Final steps
git tag v0.2.0
git push origin v0.2.0
# GitHub Actions automatically builds and releases
# Check: https://github.com/quangtv1/pfiev/actions
# Release appears on: https://github.com/quangtv1/pfiev/releases
```

---

## Contact & Support

- **Issues:** GitHub Issues tracker
- **Documentation:** See ./docs/ directory
- **Build Questions:** Contact maintainers
