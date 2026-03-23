# PhanBo-PFIEV Documentation

**Version:** v0.1.3 | **Last Updated:** 2026-03-23

Welcome to the complete documentation for PhanBo-PFIEV. This folder contains everything you need to understand, deploy, and develop the application.

---

## Quick Navigation

### For Users 👥
**New to the app? Start here:**
1. [Deployment Guide](./deployment-guide.md) - How to install and run
2. [Project Overview](./project-overview-pdr.md) - What the app does
3. [Codebase Summary](./codebase-summary.md) - Understanding the system

### For Developers 💻
**Want to contribute or extend the code? Read these:**
1. [Codebase Summary](./codebase-summary.md) - Architecture overview
2. [System Architecture](./system-architecture.md) - Detailed design
3. [Code Standards](./code-standards.md) - Coding guidelines
4. [Deployment Guide](./deployment-guide.md) - Build & release

### For Project Managers 📊
**Planning or tracking progress?**
1. [Project Overview & PDR](./project-overview-pdr.md) - Goals, requirements, strategy
2. [Project Roadmap](./project-roadmap.md) - Phases, timeline, features
3. [System Architecture](./system-architecture.md) - Technical scope

---

## Documentation Files

| File | Purpose | Audience |
|------|---------|----------|
| **[project-overview-pdr.md](./project-overview-pdr.md)** | Project vision, goals, requirements, PDR | PM, Devs, Users |
| **[codebase-summary.md](./codebase-summary.md)** | Codebase structure, modules, key components | Devs, Architects |
| **[system-architecture.md](./system-architecture.md)** | 4-layer architecture, data flow, design | Devs, Architects |
| **[code-standards.md](./code-standards.md)** | Python style, patterns, conventions | Devs |
| **[deployment-guide.md](./deployment-guide.md)** | Installation, build, CI/CD, troubleshooting | Devs, DevOps, Users |
| **[project-roadmap.md](./project-roadmap.md)** | Version history, features, phases, timeline | PM, Devs |

---

## Quick Facts

**Application:** PhanBo-PFIEV (Student Allocation System)

**Tech Stack:**
- Language: Python 3.12
- GUI: PySide6 (Qt6)
- Database: SQLite + optional Access .mdb
- Build: PyInstaller → Windows .exe
- CI/CD: GitHub Actions

**Current Version:** v0.1.3 (Stable)

**Key Features:**
- ✅ Manage student records + grades
- ✅ Import/export Excel
- ✅ Automatic allocation to programs
- ✅ Statistics & charts
- ✅ Bilingual (Vietnamese + French)
- ✅ Automated Windows build

**Repository:** https://github.com/quangtv1/pfiev

---

## Getting Started

### Install & Run (Windows Users)

```bash
# 1. Download latest release
# https://github.com/quangtv1/pfiev/releases

# 2. Extract the ZIP file
# C:\Program Files\PhanBo-PFIEV

# 3. Run the app
# Double-click PhanBo-PFIEV.exe
```

See [Deployment Guide](./deployment-guide.md) for detailed instructions.

### Setup Development Environment

```bash
# 1. Clone repository
git clone https://github.com/quangtv1/pfiev.git
cd PhanBo-PFIEV

# 2. Create virtual environment
python -m venv venv
venv\Scripts\activate  # Windows

# 3. Install dependencies
pip install -r requirements.txt

# 4. Run from source
python main.py
```

See [Deployment Guide - Developer Setup](./deployment-guide.md#developer-setup).

---

## Key Concepts

### Candidat
A student participating in the exam. Contains:
- Personal info (name, DOB, gender, status)
- Language preference (VI/FR)
- Average score (computed)
- Ranking (computed)

### Choix
Student's priority list of programs (specializations). Each choice:
- Links candidat → filiere
- Has priority order (1-5)
- Marked as admitted after allocation

### Filiere
Academic program/specialization with:
- Name, school affiliation
- Available seats (capacity)

### Attribution
Automatic allocation process:
1. Rank students by average (best first)
2. For each student, assign to first available program
3. Mark as admitted

### Session
Working context (.mdb file) containing:
- All students for a given exam period
- Their grades, choices, results
- Allocation status

---

## Architecture Highlights

```
┌─────────────────────────────────────┐
│    Presentation (Views - PySide6)   │  ← User Interface
├─────────────────────────────────────┤
│   Business Logic (Services)         │  ← Attribution, Excel handling
├─────────────────────────────────────┤
│   Data Access (Repositories)        │  ← CRUD operations
├─────────────────────────────────────┤
│   Persistence (SQLite/Access)       │  ← Database
└─────────────────────────────────────┘
```

See [System Architecture](./system-architecture.md) for detailed diagrams and flows.

---

## Common Tasks

### Import Student Data from Excel

1. Launch app → Select session
2. Click "Import Excel"
3. Select your .xlsx file
4. Verify preview
5. Click "Confirm Import"

See [Codebase Summary - Excel Service](./codebase-summary.md#excel-service-appservicesexcel_servicespy).

### Run Automatic Attribution

1. Go to main menu → Attribution
2. Verify all students have grades
3. Verify all students have choices
4. Click "Run Attribution"
5. Review results

Algorithm details: [System Architecture - Attribution Flow](./system-architecture.md#2-attribution-flow).

### Export Results to Excel

1. View results → Click "Export"
2. Choose location to save
3. Open .xlsx file with Excel

See [Code Standards - Service Layer](./code-standards.md#service-layer).

### Build Windows Executable

```bash
# Setup
pip install pyinstaller
python scripts/create_dbs.py

# Build
pyinstaller PhanBo-PFIEV.spec

# Output: dist/PhanBo-PFIEV/PhanBo-PFIEV.exe
```

See [Deployment Guide - Building from Source](./deployment-guide.md#building-from-source).

---

## Troubleshooting

**App won't start?**
→ Check [Deployment Guide - Troubleshooting](./deployment-guide.md#troubleshooting)

**Build fails?**
→ Check [Deployment Guide - Build Issues](./deployment-guide.md#troubleshooting-build-issues)

**Import Excel doesn't work?**
→ Verify file format in [Code Standards - Excel Service](./code-standards.md)

**Attribution produces unexpected results?**
→ Review algorithm in [System Architecture - Attribution Service](./system-architecture.md#attribution-service-appservicesattribution_servicespy)

**Can't find a feature?**
→ Check [Project Roadmap](./project-roadmap.md) - might be planned for v0.2+

---

## Development Workflow

### Code Review Checklist

Before committing, verify:
- [ ] Type hints on all functions
- [ ] Docstrings on public methods
- [ ] No hardcoded strings (use L.get())
- [ ] SQL queries parameterized
- [ ] Error handling for user actions

See [Code Standards - Code Review Checklist](./code-standards.md#code-review-checklist).

### Testing

```bash
# Install test tools
pip install pytest pytest-cov

# Run tests (currently minimal)
pytest tests/

# With coverage
pytest --cov=app tests/
```

### Building Release

```bash
# Tag release
git tag v0.2.0
git push origin v0.2.0

# GitHub Actions automatically:
# 1. Builds .exe
# 2. Creates release
# 3. Uploads artifact
```

See [Deployment Guide - GitHub Actions CI/CD](./deployment-guide.md#github-actions-cicd).

---

## Contributing

### Report a Bug

1. Go to: https://github.com/quangtv1/pfiev/issues
2. Click "New Issue"
3. Describe the problem + reproduction steps
4. Include screenshots if relevant

### Request a Feature

1. Check [Project Roadmap](./project-roadmap.md)
2. Open GitHub Issue with details
3. Priority determined by team

### Contribute Code

1. Fork repository
2. Create feature branch
3. Follow [Code Standards](./code-standards.md)
4. Submit pull request
5. Await review

(Formal contribution guidelines coming soon)

---

## Glossary

| Term | Definition |
|------|-----------|
| **Candidat** | Student |
| **Choix** | Choice/preference of program |
| **Filiere** | Academic program/specialization |
| **Établissement** | School/institution |
| **Attribution** | Allocation/assignment of students to programs |
| **Session** | Working context for a given exam period |
| **Moyenne** | Average score |
| **Concours** | Examination/contest |
| **Anonymat** | Anonymous ID |
| **PySide6** | Python Qt6 binding |

See [Project Overview & PDR - Glossary](./project-overview-pdr.md#glossary) for more terms.

---

## Version & Updates

**Current Version:** v0.1.3 (stable)
**Release Date:** 2026-03-23
**Next Major Release:** v0.2.0 (planned Q2 2026)

See [Project Roadmap](./project-roadmap.md) for detailed version history and planned features.

---

## Key Resources

| Resource | Link |
|----------|------|
| **GitHub Repository** | https://github.com/quangtv1/pfiev |
| **Releases & Downloads** | https://github.com/quangtv1/pfiev/releases |
| **Issue Tracker** | https://github.com/quangtv1/pfiev/issues |
| **Code** | https://github.com/quangtv1/pfiev (main branch) |

---

## Getting Help

**For Questions:**
- Check relevant documentation file (see table above)
- Search GitHub Issues
- Open new GitHub Issue with detailed question

**For Bug Reports:**
- GitHub Issues with reproduction steps
- Include error logs if available
- Specify Windows version

**For Feature Requests:**
- GitHub Issues with detailed description
- Include use case and expected behavior

---

## Documentation Standards

This documentation follows these principles:

1. **Accuracy:** All information verified against actual code
2. **Clarity:** Written for target audience (users, devs, managers)
3. **Completeness:** Covers features, architecture, deployment
4. **Maintainability:** Updated with each release
5. **Accessibility:** Clear navigation, good cross-referencing

---

## License & Credits

**PhanBo-PFIEV**
- Original concept: VB6 + C# version
- Python 3.12 + PySide6 rewrite
- Open source (license: TBD)

---

## Last Updated

**2026-03-23** - Complete documentation for v0.1.3

For updates, check the "Last Updated" line in each document file.

---

## Questions?

Open an issue on GitHub: https://github.com/quangtv1/pfiev/issues

Happy using & developing! 🚀
