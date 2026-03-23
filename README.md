# PhanBo-PFIEV

Phần mềm hỗ trợ **phân bổ thí sinh vào chuyên ngành** trong chương trình kỹ sư PFIEV (Pháp - Việt). Được viết bằng Python 3.12 + PySide6, thay thế hoàn toàn phiên bản VB6 gốc.

---

## Chức năng

### 1. Quản lý cấu hình hệ thống
- Quản lý danh sách **môn thi** (mã, tên, hệ số)
- Quản lý danh sách **trường** (mã, tên)
- Quản lý danh sách **chuyên ngành** (mã, tên, trường, số chỗ)
- Đặt số chỗ chung cho tất cả chuyên ngành cùng lúc
- Cài số lượng ngành tối đa mỗi thí sinh được chọn

### 2. Quản lý phiên thi
- Tạo phiên thi mới từ template cơ sở dữ liệu
- Mở phiên thi đã có (file `.mdb`)
- Mỗi phiên thi lưu độc lập, không ảnh hưởng các phiên khác

### 3. Quản lý thí sinh
- Thêm / sửa / xóa thí sinh trong phiên thi
- Thông tin thí sinh: họ tên, họ lót, ngày sinh, giới tính, ngôn ngữ, trường, số ẩn danh, trạng thái (Interne / Externe)
- Hiển thị danh sách đầy đủ dưới dạng bảng với tìm kiếm và sắp xếp

### 4. Nhập điểm
- Nhập điểm từng môn thi cho từng thí sinh
- Hệ thống tự động tính điểm trung bình có trọng số theo hệ số môn
- Xếp hạng thí sinh theo điểm trung bình

### 5. Chọn ngành ưu tiên
- Thí sinh đăng ký tối đa N ngành theo thứ tự ưu tiên (N cấu hình được)
- Giao diện kéo thả ngành từ danh sách khả dụng sang danh sách đã chọn
- Điều chỉnh thứ tự ưu tiên bằng nút lên/xuống

### 6. Thuật toán phân bổ
- Chạy phân bổ tự động dựa trên điểm trung bình và thứ tự ưu tiên của thí sinh
- Ưu tiên thí sinh điểm cao hơn trong cùng ngành
- Tôn trọng số chỗ giới hạn của từng chuyên ngành
- Phân biệt Interne / Externe trong quá trình phân bổ

### 7. Xem kết quả
- Bảng kết quả phân bổ: xếp hạng, họ tên, điểm TB, ngành được nhận
- Bảng tóm tắt theo ngành: số Interne / Externe / tổng
- Đổi ngành sau khi phân bổ (trường hợp ngoại lệ)

### 8. Thống kê
- **Thống kê tổng quát**: số thí sinh theo ngành, tỉ lệ lấp đầy
- **Thống kê theo trường**: phân bổ Interne / Externe từng trường
- **Phân bố điểm TB**: biểu đồ phân bổ theo khoảng điểm
- **Điểm TB theo ngành**: so sánh điểm đầu vào các chuyên ngành
- **Top thí sinh theo ngành**: danh sách thí sinh tốt nhất từng ngành
- **Thống kê theo ngôn ngữ**: phân bổ theo ngôn ngữ học (Pháp/Việt)

### 9. Biểu đồ
- Biểu đồ cột: số thí sinh được nhận theo từng ngành
- Biểu đồ tròn: tỉ lệ Interne / Externe toàn phiên

### 10. Nhập / Xuất Excel
- **Nhập**: import danh sách thí sinh từ file `.xlsx`
- **Xuất danh sách thí sinh**: file Excel kèm ngày xuất trong tên file
- **Xuất kết quả phân bổ**: file Excel tổng hợp kết quả

### 11. Song ngữ
- Hỗ trợ giao diện **Tiếng Việt** và **Tiếng Pháp**
- Chuyển ngôn ngữ không cần khởi động lại

---

## Công nghệ

| Thành phần | Công nghệ |
|-----------|-----------|
| Giao diện | Python 3.12 + PySide6 (Qt6) |
| Cơ sở dữ liệu | SQLite (file `.mdb` / `.db`) |
| Excel | openpyxl |
| Biểu đồ | matplotlib |
| Đóng gói | PyInstaller → Windows `.exe` |
| CI/CD | GitHub Actions |
