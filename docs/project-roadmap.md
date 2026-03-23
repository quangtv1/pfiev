# Project Roadmap - OrientationPFIEV-Python

**Current Status:** v0.1.3 (Stable) | **Last Updated:** 2026-03-23 | **Maintainer:** TBD

---

## Version History

### v0.1.3 (Current - 2026-03-23) ✅ STABLE

**Status:** Production Ready

**Features:**
- ✅ Session management (create, open, manage)
- ✅ Candidate CRUD (add, edit, delete, view)
- ✅ Bulk import from Excel (.xlsx)
- ✅ Subject & grade management
- ✅ Filiere (specialization) setup + capacity
- ✅ Candidate choice input (≤ 5 priorities)
- ✅ Automatic attribution algorithm (ranking → allocation)
- ✅ Results export to Excel
- ✅ Statistics & charts (6+ views)
- ✅ Bilingual UI (Vietnamese & French)
- ✅ PyInstaller build pipeline
- ✅ GitHub Actions CI/CD (auto .exe build)
- ✅ SQLite + legacy Access .mdb support

**Quality:**
- Clean architecture (4 layers: UI, Services, Repos, DB)
- Type hints on all functions
- Comprehensive docstrings
- Error handling for user actions
- Localization ready

**Known Limitations:**
- No authentication (local app only)
- Single-threaded (no async operations)
- ~200-300 student capacity before UI lag
- No audit logging
- No data encryption

**Bug Fixes in v0.1.3:**
- Fixed attribution algorithm for edge cases
- Improved Excel import validation
- Better error messages
- Database connection stability

---

### v0.1.2 (2026-03-10) ✅ ARCHIVED

**Features:** Initial working release
- Basic session management
- Candidate + grade input
- Simple attribution
- Excel export

**Status:** Superseded by v0.1.3

---

## Release Timeline

```
v0.1.0 (2026-02-15) → v0.1.1 → v0.1.2 → v0.1.3 (CURRENT)
                                                      ↓
                                        [Next: v0.2.0]
```

---

## Roadmap: Phases & Features

### Phase 1: CURRENT (v0.1.3) ✅

**Objective:** Stable core functionality for pilot use

**Timeline:** Completed

**Achievements:**
- Solid foundation
- Working attribution algorithm
- Basic analytics
- Automated build pipeline
- User feedback collected

**Metrics:**
- Zero critical bugs (v0.1.3)
- 100% core features working
- < 5 min attribution for 100 students
- Successful pilot deployment

---

### Phase 2: ENHANCEMENT (v0.2.0) ⏳ PLANNED

**Objective:** Improve usability, add advanced features

**Timeline:** Q2 2026 (April - June)

**Estimated Effort:** 3-4 weeks

#### Features

**R2.1: Enhanced Attribution**
- [ ] Support weighted scoring (not just average)
- [ ] Admin override capabilities
- [ ] Conflict detection & resolution UI
- [ ] Re-run attribution (update grades, re-allocate)
- [ ] Allocation preview before commit
- [ ] Undo/restore allocation history

**R2.2: Improved Import/Export**
- [ ] Import template generator (Excel)
- [ ] Batch candidate photos upload (future)
- [ ] Multiple export formats (PDF, CSV)
- [ ] Custom report builder

**R2.3: Analytics Enhancement**
- [ ] Dashboard KPIs (acceptance rate, avg per filiere)
- [ ] Year-on-year trend analysis
- [ ] Predictive analytics (estimate allocation)
- [ ] Export detailed reports (PDF)

**R2.4: User Experience**
- [ ] Dark mode support
- [ ] Keyboard shortcuts
- [ ] Multi-language: Add Spanish, Chinese
- [ ] Localized number formatting

**R2.5: Testing & Quality**
- [ ] Unit tests (core logic)
- [ ] Integration tests (DB operations)
- [ ] UI automation tests
- [ ] Performance benchmarks

**R2.6: Documentation**
- [ ] User manual (PDF, bilingual)
- [ ] Video tutorials
- [ ] FAQ & troubleshooting guide
- [ ] API documentation

**Acceptance Criteria:**
- All unit tests pass (>80% coverage)
- Manual QA on Windows 10+
- No critical bugs in release candidate
- User manual complete & reviewed

---

### Phase 3: SECURITY & SCALE (v0.3.0) ⏳ PLANNED

**Objective:** Production-ready security, support larger datasets

**Timeline:** Q3-Q4 2026 (July - December)

**Estimated Effort:** 5-6 weeks

#### Features

**R3.1: Data Security**
- [ ] Encrypt .mdb files at rest (AES-256)
- [ ] Local authentication (password)
- [ ] Role-based access control (admin/teacher/viewer)
- [ ] Audit logging (who changed what, when)
- [ ] Session lockdown (read-only after attribution)

**R3.2: Performance & Scale**
- [ ] Support 1000+ students per session
- [ ] Database indexing optimization
- [ ] Query result pagination
- [ ] Async operations (import, export, attribution)
- [ ] Progress bars & cancellation support

**R3.3: Data Integrity**
- [ ] Transaction rollback on error
- [ ] Duplicate detection + merge UI
- [ ] Data validation rules
- [ ] Constraint enforcement
- [ ] Automatic backups (before operations)

**R3.4: Advanced Features**
- [ ] Multi-session management
- [ ] Batch session processing
- [ ] Custom scoring algorithms
- [ ] Preference override rules
- [ ] Fairness algorithms (diversity scoring)

**Acceptance Criteria:**
- 1000 student test dataset works smoothly
- Encryption tested with real Access .mdb
- Audit log captures all mutations
- Performance: attribution < 10 sec for 1000 HS

---

### Phase 4: ENTERPRISE (v1.0.0) 🔮 FUTURE

**Objective:** Enterprise-grade platform

**Timeline:** 2027+ (Long-term)

**Estimated Effort:** 8+ weeks

#### Features

**R4.1: Integration**
- [ ] REST API for external systems
- [ ] LDAP/AD user directory
- [ ] Cloud backup (AWS S3, OneDrive)
- [ ] Database migration (PostgreSQL support)
- [ ] Webhook notifications

**R4.2: Advanced Analytics**
- [ ] Machine learning: predict allocation outcomes
- [ ] Fairness metrics (diversity index)
- [ ] Bottleneck analysis
- [ ] Recommendation engine

**R4.3: Deployment**
- [ ] Web version (Flask/FastAPI backend)
- [ ] Mobile app (iOS/Android)
- [ ] Docker containers
- [ ] Multi-platform builds (macOS, Linux)
- [ ] Code signing for .exe (Authenticode)

**R4.4: Compliance**
- [ ] GDPR compliance (data deletion, export)
- [ ] Accessibility (WCAG 2.1)
- [ ] Internationalization (10+ languages)
- [ ] Data retention policies

---

## Feature Priority Matrix

| Priority | Feature | Effort | Impact | Status |
|----------|---------|--------|--------|--------|
| P0 (Critical) | Core attribution | Done | High | ✅ v0.1.3 |
| P0 | Session management | Done | High | ✅ v0.1.3 |
| P1 (High) | Weighted scoring | M | High | ⏳ v0.2 |
| P1 | Performance scale | M | High | ⏳ v0.3 |
| P1 | Data encryption | M | High | ⏳ v0.3 |
| P2 (Medium) | Dark mode | S | Low | 🔮 v0.2 |
| P2 | REST API | L | Medium | 🔮 v1.0 |
| P3 (Low) | Mobile app | XL | Low | 🔮 v1.0 |

**Effort:** S=Small, M=Medium, L=Large, XL=Extra Large

---

## Current Sprint (v0.2.0)

**Goal:** Enhanced attribution + analytics

**Timeline:** 2026-04-01 to 2026-05-31 (8 weeks)

### Tasks

#### Week 1-2: Design & Planning
- [ ] Spec weighted scoring algorithm
- [ ] Design analytics dashboard wireframes
- [ ] Plan test coverage strategy
- [ ] Review user feedback

#### Week 3-4: Core Development
- [ ] Implement weighted scoring
- [ ] Add override UI
- [ ] Build analytics queries
- [ ] Unit test core logic

#### Week 5-6: Testing & QA
- [ ] Integration tests
- [ ] Manual QA
- [ ] Performance testing
- [ ] Bug fixes

#### Week 7-8: Documentation & Release
- [ ] Write user manual
- [ ] Update technical docs
- [ ] Create video tutorials
- [ ] Release v0.2.0

---

## Backlog (Future Consideration)

### Unscheduled Features

- [ ] Batch session processing
- [ ] Custom report builder
- [ ] Predictive analytics
- [ ] Machine learning scoring
- [ ] Mobile app
- [ ] Web version
- [ ] API server
- [ ] Real-time collaboration (multi-user)

---

## Known Issues & Improvements

### Bugs (Minor)

| Issue | Severity | Workaround | Target Fix |
|-------|----------|-----------|-----------|
| Typo in French menu | Low | Use Vietnamese | v0.2.0 |
| Chart rendering slow (500+ data) | Medium | Limit chart to top 50 | v0.3.0 |
| Excel import: date format validation | Medium | Manual format check | v0.2.0 |

### Technical Debt

| Item | Reason | Priority | Effort |
|------|--------|----------|--------|
| Remove legacy VB6 compatibility | Simplify codebase | Medium | M |
| Add comprehensive logging | Debugging | Medium | M |
| Refactor large form files | Code organization | Low | M |
| Add integration tests | Quality assurance | High | L |

---

## Success Metrics

### v0.2.0 Targets

| Metric | Current | Target |
|--------|---------|--------|
| Build time | 2 min | < 2 min |
| Attribution speed (100 HS) | 3 sec | < 2 sec |
| UI responsiveness | Fair | Good |
| Test coverage | 0% | > 50% |
| Documentation completeness | 60% | 90% |
| User satisfaction | Good | Excellent |

### v1.0.0 Targets

| Metric | Target |
|--------|--------|
| Performance (1000 HS) | < 10 sec attribution |
| Uptime | 99.9% |
| Test coverage | > 80% |
| Security | GDPR compliant |
| Scale | 10,000+ annual users |

---

## Dependencies & Blockers

### External Dependencies

| Dependency | Purpose | Status |
|------------|---------|--------|
| Python 3.12 | Runtime | ✅ Stable |
| PySide6 6.7+ | GUI | ✅ Stable |
| openpyxl | Excel | ✅ Stable |
| matplotlib | Charts | ✅ Stable |
| SQLite 3.x | DB | ✅ Stable |
| PyInstaller 6.x | Build | ✅ Stable |

### Version Upgrade Plan

- **Annual review** of dependency versions
- **Security patches** applied immediately
- **Major upgrades** planned in roadmap phases

---

## Communication & Feedback

### User Feedback Channels

1. **GitHub Issues** - Bug reports, feature requests
2. **Email** - Direct feedback from administrators
3. **User surveys** - Quarterly feedback (v0.2+)
4. **Beta testing** - Early access program (v0.3+)

### Release Notes Template

```markdown
## v0.2.0 Release Notes

### New Features
- Weighted scoring algorithm
- Analytics dashboard
- Enhanced import wizard

### Bug Fixes
- Fixed attribution edge case
- Improved error handling

### Breaking Changes
- None

### Migration Guide
- No data migration needed

### Known Issues
- Chart rendering slow for 500+ data points

### Contributors
- @user1 (feature)
- @user2 (bugfix)
```

---

## Stakeholder Alignment

| Stakeholder | Goals | Metrics |
|-------------|-------|---------|
| **Users** | Easy to use, accurate results | Satisfaction score > 4/5 |
| **Admins** | Reliable, secure, scalable | Zero data loss, 99.9% uptime |
| **Developers** | Clean code, good docs | Code coverage > 80% |
| **Org** | Cost-effective, future-proof | Low maintenance, extensible |

---

## Budget & Resources

### Development Team

- **Lead Developer:** 1 FTE
- **QA Tester:** 0.5 FTE (part-time)
- **Product Manager:** 0.25 FTE (part-time)
- **Documentation:** Volunteer

### Infrastructure

- **GitHub:** Free (open source)
- **CI/CD:** GitHub Actions (free)
- **Code Review:** Peer review
- **Testing:** Manual + automated

### Cost Estimate

- **v0.1.x → v0.2.0:** 3-4 weeks development
- **v0.2.0 → v0.3.0:** 5-6 weeks development
- **v0.3.0 → v1.0.0:** 8+ weeks development
- **Annual maintenance:** 2-3 weeks/year

---

## Open Questions

1. **Multi-user support?** Currently single-user desktop. Web/API version?
2. **Mobile app?** iOS/Android client or just admin dashboard?
3. **Data retention?** How long to keep historical sessions?
4. **Licensing?** Open source? Commercial?
5. **Internationalization?** Support more languages?
6. **Integration?** Connect to school management systems?

---

## Next Steps

1. **v0.2.0 Planning** (Next sprint)
   - Prioritize user feedback
   - Detailed spec for weighted scoring
   - Design analytics UI

2. **User Testing** (Next month)
   - Collect feedback from pilot deployment
   - Identify pain points
   - Prioritize improvements

3. **Documentation Audit** (Ongoing)
   - User manual draft
   - Video tutorials
   - FAQ

---

## Contact

- **Questions:** Open GitHub Issue
- **Feedback:** Email team
- **Reporting Bugs:** GitHub Issues
- **Contributing:** See CONTRIBUTING.md (future)
