# Project Overview & PDR - OrientationPFIEV-Python

**Current Version:** v0.1.3 | **Status:** Active Development | **Last Updated:** 2026-03-23

---

## Executive Summary

**OrientationPFIEV** là hệ thống quản lý & phân bổ học sinh (orientation) dành cho kỳ thi PFIEV (Programme de Formation aux Innovations en Éducation et Vocation). Ứng dụng hỗ trợ:

✅ Quản lý hồ sơ học sinh + điểm số
✅ Import/export dữ liệu Excel
✅ Lựa chọn ngành học theo ưu tiên
✅ Phân bổ tự động dựa trên xếp hạng + điểm trung bình
✅ Thống kê & biểu đồ kết quả
✅ Đa ngôn ngữ: Tiếng Việt + Tiếng Pháp

---

## Product Vision & Goals

### Long-term Vision
Trở thành nền tảng toàn diện cho quản lý tuyển sinh & định hướng học sinh với:
- Tự động hóa quy trình phân bổ
- Tối ưu hóa công bằng (fairness-based allocation)
- Hỗ trợ quyết định dựa trên dữ liệu (analytics)
- Mở rộng sang quản lý tuyển sinh đại học

### Core Success Metrics
- **Usability:** Users có thể hoàn thành phân bổ trong < 5 phút
- **Accuracy:** 100% match với xếp hạng + lựa chọn ưu tiên
- **Coverage:** Support 500+ students/session (v1.0 target)
- **Reliability:** Zero data loss, automatic backups

---

## Functional Requirements

### 1. Quản Lý Dữ Liệu Cơ Bản

#### R1.1: Quản lý học sinh (Candidat)
- [ ] Thêm/xóa/sửa học sinh
- [ ] Lưu trữ: Họ tên, ngày sinh, giới tính, trạng thái (nội/ngoài trường)
- [ ] Hỗ trợ mã ẩn danh (anonymat)
- [ ] Import/export Excel (batch)

#### R1.2: Quản lý môn học & điểm số
- [ ] Định nghĩa danh sách môn học
- [ ] Nhập điểm từng học sinh
- [ ] Tính tự động điểm trung bình
- [ ] Hỗ trợ import từ file Excel

#### R1.3: Quản lý ngành học & trường
- [ ] Định nghĩa ngành học (tên, số chỗ)
- [ ] Gán ngành → trường
- [ ] Điều chỉnh số chỗ động

#### R1.4: Quản lý kỳ thi & điều kiện
- [ ] Tạo kỳ thi (năm, điểm tối thiểu)
- [ ] Cấu hình tham số (điều kiện tham gia)

---

### 2. Quản Lý Lựa Chọn & Phân Bổ

#### R2.1: Lựa chọn ngành học
- [ ] Học sinh chọn ≤ 5 ngành (ưu tiên)
- [ ] Lưu trữ thứ tự ưu tiên (choix_ordre)
- [ ] Cập nhật động danh sách lựa chọn

#### R2.2: Phân bổ tự động (Attribution)
- [ ] Xếp hạng học sinh theo điểm trung bình (tốt nhất trước)
- [ ] Phân bổ tuần tự: mỗi HS → ngành đầu tiên có chỗ
- [ ] Ghi lại trạng thái (choix_admis = true/false)
- [ ] Xuất danh sách kết quả

#### R2.3: Điều chỉnh thủ công
- [ ] Thay đổi tay phân bổ của HS
- [ ] Xóa/khôi phục phân bổ
- [ ] Kiểm tra xung đột (capacity validation)

---

### 3. Import & Export

#### R3.1: Import Excel
- [ ] Hỗ trợ file .xlsx
- [ ] Mapping cột: Họ, Tên, Ngày sinh, Giới tính, Điểm môn, ...
- [ ] Validation + error reporting
- [ ] Batch insert → database

#### R3.2: Export kết quả
- [ ] Danh sách học sinh → Excel (Thi sinh sheet)
- [ ] Danh sách kết quả phân bổ → Excel (Ket qua sheet)
- [ ] Định dạng chuyên nghiệp (header, màu sắc)

---

### 4. Thống Kê & Phân Tích

#### R4.1: Báo cáo & biểu đồ
- [ ] Phân bổ theo ngành (count + percentage)
- [ ] Phân bổ theo trường (groupby)
- [ ] Phân布 điểm trung bình (histogram)
- [ ] Tỷ lệ chấp nhận (acceptance rate)

#### R4.2: Bảng tóm tắt (Tableau Récap)
- [ ] Hiển thị: HS, điểm TB, ngành được phân bổ, trạng thái
- [ ] Filter, sort, search
- [ ] Xuất Excel được chọn

---

### 5. Cấu hình & Hệ thống

#### R5.1: Quản lý phiên làm việc (Session)
- [ ] Tạo session mới (tạo .mdb)
- [ ] Mở session cũ
- [ ] Backup/restore dữ liệu

#### R5.2: Cấu hình ứng dụng
- [ ] Chọn ngôn ngữ (VI/FR)
- [ ] Đặt tham số hệ thống
- [ ] Quản lý template database

#### R5.3: Validation & Error Handling
- [ ] Check dữ liệu không hợp lệ (null, ký tự đặc biệt)
- [ ] Confirm trước hành động nguy hiểm (xóa bulk)
- [ ] Ghi log lỗi

---

## Non-Functional Requirements

### NFR1: Performance
- **Load time:** Mở session < 3 giây
- **Attribution runtime:** < 10 giây cho 500 HS
- **Export:** < 5 giây cho 1000 rows
- **DB query:** < 1 giây cho read operations

### NFR2: Reliability
- **Uptime:** 99.9% trong production
- **Data integrity:** Foreign keys enforced, transactions rolled back on error
- **Backup:** Auto-backup trước mỗi attribution run

### NFR3: Usability
- **Language:** VI & FR, switchable without restart
- **UI/UX:** Wizard-based workflow, clear error messages
- **Accessibility:** Standard Windows font sizes, high-contrast option

### NFR4: Security
- **Data:** Encrypt .mdb files at rest (Future v1.0)
- **Access:** No user authentication (local-only app)
- **Audit:** Log all attribution changes (Future)

### NFR5: Maintainability
- **Code:** Python 3.12, PySide6 (latest)
- **DB:** SQLite + optional Access support
- **Docs:** API docs + user manual
- **Testing:** Unit tests for core logic (Future)

---

## Architecture Decisions

### AD1: Desktop App vs. Web
**Decision:** Desktop (Windows .exe)
**Rationale:**
- No internet required (works offline)
- Direct file system access (Excel import/export)
- Simple deployment (single .exe)
- Legacy Access .mdb support

### AD2: Database Strategy
**Decision:** Dual support: SQLite (primary) + Access .mdb (legacy)
**Rationale:**
- config.db = SQLite (fast, lightweight)
- Session .mdb = SQLite variant (compatible with VB6/C# legacy)
- pyodbc adapter = fallback for real Access on Windows with driver

### AD3: UI Framework
**Decision:** PySide6 (Qt6)
**Rationale:**
- Modern, professional UI
- Cross-platform ready (future macOS/Linux)
- No licensing issues (LGPL)
- Large community support

### AD4: Deployment
**Decision:** PyInstaller → .exe + GitHub Actions CI/CD
**Rationale:**
- Single-file executable (users don't need Python)
- Automated build on tag push
- GitHub Releases for distribution
- Easy rollback (version history)

---

## Current Features (v0.1.3)

✅ **Core Completed:**
- [x] Session management (create, open)
- [x] Candidate CRUD (add, edit, delete, bulk import)
- [x] Subject & grade management
- [x] Filiere & establishment setup
- [x] Candidate choice input (≤ 5 priorities)
- [x] Automatic attribution (ranking + allocation)
- [x] Results export (Excel)
- [x] Statistics & charts (6+ views)
- [x] Bilingual UI (VI/FR)
- [x] PyInstaller build → Windows .exe

✅ **Tested & Stable:**
- Core attribution algorithm (ported from C#)
- Excel import/export (openpyxl)
- Database schema (Candidat, Choix, Filiere, etc.)
- Localization system

---

## Planned Features (v0.2+)

### v0.2: Enhanced Attribution
- [ ] Weighted scoring (not just average)
- [ ] Override rules (admin can force allocation)
- [ ] Conflict detection & resolution UI
- [ ] Re-run attribution (update grades, re-allocate)

### v0.3: Analytics & Reporting
- [ ] Dashboard (KPIs: acceptance rate, avg per filiere, etc.)
- [ ] Export detailed reports (PDF)
- [ ] Trend analysis (year-on-year)
- [ ] Predictive: estimate allocation before run

### v0.4: Data Security
- [ ] Encrypt .mdb files (AES-256)
- [ ] User authentication (local password)
- [ ] Audit log (who, what, when)
- [ ] Role-based access (admin, teacher, viewer)

### v1.0: Scale & Polish
- [ ] Support 1000+ students/session
- [ ] Batch export (multiple sessions)
- [ ] Integration: LDAP/AD for users
- [ ] Cloud backup option
- [ ] Unit tests + integration tests
- [ ] macOS/Linux builds

---

## Risk Assessment

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|-----------|
| Attribution algorithm bug | High | Medium | Unit tests, manual verification |
| Data loss (session .mdb) | Critical | Low | Auto-backup, recovery tools |
| .mdb file corruption | High | Low | SQLite primary, validation on open |
| Excel import format mismatch | Medium | High | Template + validation wizard |
| Access driver not available | Medium | Low | Fallback to SQLite |
| Large dataset (500+ HS) performance | Medium | Medium | Query optimization, indexing |

---

## Success Criteria

### Phase 1 (Current - v0.1.3)
- ✅ Core features functional
- ✅ Attribution algorithm works correctly
- ✅ Build pipeline automated
- ✅ Bilingual support
- ⏳ User feedback incorporated

### Phase 2 (v0.2)
- [ ] Enhanced features + bug fixes
- [ ] User manual complete
- [ ] 100% unit test coverage (core logic)

### Phase 3 (v1.0)
- [ ] Enterprise-ready (scale, security, audit)
- [ ] API documentation
- [ ] Multi-language support (3+ languages)

---

## Acceptance Criteria

For any feature to be considered "done":

1. **Functional:** Feature works as per spec
2. **Tested:** Unit tests + manual QA pass
3. **Documented:** Code comments + user docs
4. **Integrated:** No breaking changes to existing features
5. **Performance:** Meets NFR thresholds
6. **Localized:** All UI text in L class (i18n-ready)

---

## Release & Deployment Strategy

### Version Format
`vX.Y.Z` (semantic versioning)
- X = Major (breaking changes)
- Y = Minor (features)
- Z = Patch (fixes)

### Release Process
1. Merge to `main` branch
2. Tag: `git tag vX.Y.Z`
3. Push tag → GitHub Actions triggered
4. Build .exe → Release artifact created
5. Announce on repo releases page

### Rollback Plan
- GitHub Releases page shows all versions
- Users can download older .zip if needed
- No database migrations (backward compatible)

---

## Communication & Stakeholders

| Role | Contact | Frequency |
|------|---------|-----------|
| Product Owner | TBD | Monthly review |
| Developers | Team | Daily standup |
| Users/Testers | TBD | Feedback on release |
| IT/DevOps | TBD | Build + deployment |

---

## Key Contacts & Resources

- **Repository:** https://github.com/quangtv1/pfiev
- **Issues:** GitHub Issues for bugs + features
- **Documentation:** ./docs/ directory
- **Build:** .github/workflows/build.yml (GitHub Actions)

---

## Glossary

| Term | Definition |
|------|-----------|
| **Candidat** | Học sinh tham gia thi |
| **Choix** | Lựa chọn ngành (ưu tiên) của HS |
| **Filiere** | Ngành học / Chuyên ngành |
| **Établissement** | Trường học / Cơ sở giáo dục |
| **Attribution** | Phân bổ HS vào ngành |
| **Session** | Phiên làm việc (kỳ thi) |
| **Moyenne** | Điểm trung bình |
| **Concours** | Kỳ thi |
