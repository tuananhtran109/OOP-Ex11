# Campus Safety Hub - Báo Cáo Thực Hành OOP Tuần 11

/****************/  
MSSV: 202418834  
Họ tên: Trần Tuấn Anh  
GitHub Repository: https://github.com/tuananhtran109/OOP-Ex11  
/****************/  

---

## 1. Giới thiệu dự án
Dự án **Campus Safety Hub** là hệ thống giám sát an toàn và ứng phó khẩn cấp học đường được xây dựng trên nền tảng **C# / .NET 10** theo chuẩn Lập trình Hướng đối tượng (OOP) và áp dụng 3 Mẫu thiết kế (Design Patterns) kinh điển từ Gang of Four (GoF):
1. **Adapter Pattern (Object Adapter)**
2. **Observer Pattern**
3. **Mediator Pattern**

Hệ thống giải quyết trọn vẹn bài toán tích hợp thiết bị phần cứng cũ, xử lý ngưỡng cảnh báo độc lập, phát tán sự kiện đa kênh và điều phối ứng phó khẩn cấp tập trung mà không làm tăng độ liên kết (tight coupling) giữa các thành phần.

---

## 2. Kiến trúc và 3 Design Patterns

```
+---------------------+           +------------------------+
| LegacySmokeDetector | (0..1000) | SmokeSensorAdapter     |
|   (Legacy Hardware) |<----------| (Object Adapter)       |
+---------------------+  Composition +-----------+------------+
                                                 | implements
                                                 v
                                          +--------------+
                                          |   ISensor    |
                                          +-------+------+
                                                  | reads
                                                  v
                                          +------------------+
                                          |  SensorMonitor   | (Subject)
                                          +--------+---------+
                                                   | notifies
                                                   v
                                          +------------------+
                                          |  IAlertObserver  |
                                          +--------+---------+
                                                   |
       +-----------------------+-------------------+-----------------------+
       |                       |                   |                       |
       v                       v                   v                       v
+---------------+     +------------------+  +---------------+  +--------------------------+
|   Dashboard   |     |    EmailAlert    |  |   AuditLog    |  |  EmergencyBridgeObserver |
|   Observer    |     |     Observer     |  |   Observer    |  +------------+-------------+
+---------------+     +------------------+  +---------------+               | triggers
                                                                            | (CRITICAL)
                                                                            v
                                                                +-------------------------+
                                                                |   EmergencyMediator     |
                                                                +------------+------------+
                                                                             | controls
                                            +--------------------------------+--------------------------------+
                                            |                                |                                |
                                            v                                v                                v
                                    +---------------+                +----------------+              +-------------------+
                                    |     Alarm     |                | DoorController |              |   Ventilation     |
                                    +---------------+                +----------------+              +-------------------+
```

### 2.1. Adapter Pattern (Object Adapter)
- **Mục tiêu:** Tích hợp `LegacySmokeDetector` (thang đo raw $0 \dots 1000$, đơn vị obscure) vào hệ thống mà không được sửa đổi mã nguồn phần cứng cũ.
- **Giải pháp:** Sử dụng **Object Adapter** (`SmokeSensorAdapter`), đóng gói một tham chiếu đến `LegacySmokeDetector` qua quan hệ Composition. Adapter hiện thực giao diện chuẩn `ISensor`:
  - `ReadValue()`: Quy đổi tuyến tính $\text{Percent} = \frac{\text{raw}}{1000.0} \times 100.0$.
  - `GetUnit()`: Chuẩn hóa đơn vị thành `"% smoke"`.

### 2.2. Observer Pattern
- **Mục tiêu:** Cho phép `SensorMonitor` (Subject) theo dõi các cảm biến, phân loại ngưỡng cảnh báo và phát tán sự kiện `AlertEvent` đến vô số bộ phận xử lý mà không cần biết định danh cụ thể của từng bộ phận.
- **Quy tắc phân ngưỡng:**
  - $\text{Giá trị} < 50\%$: Mức **Normal** (ghi nhận trạng thái an toàn, không phát tán cảnh báo).
  - $50\% \le \text{Giá trị} < 70\%$: Mức **WARNING** (phát tán `AlertEvent` mức cảnh báo).
  - $\text{Giá trị} \ge 70\%$: Mức **CRITICAL** (phát tán `AlertEvent` mức khẩn cấp).
- **Tính năng nâng cao:**
  - Deduplication: Chống đăng ký trùng lặp một Observer nhiều lần.
  - Snapshot iteration: Sử dụng `.ToArray()` khi duyệt danh sách thông báo để tránh `InvalidOperationException` nếu Observer gọi `Detach()` trong lúc nhận tin.
  - Observer Fault Isolation: Bọc từng lệnh `Update(evt)` trong khối `try-catch`, đảm bảo một Observer gặp sự cố không làm sập toàn bộ hệ thống.

### 2.3. Mediator Pattern
- **Mục tiêu:** Xóa bỏ lưới phụ thuộc đa hướng ($O(N^2)$) giữa các thiết bị ngoại vi ứng phó khẩn cấp (`Alarm`, `DoorController`, `VentilationController`).
- **Giải pháp:** `EmergencyMediator` đóng vai trò điều phối trung tâm hình sao ($O(N)$). Khi nhận sự kiện `"FIRE_DETECTED"`:
  1. Hú còi báo động khẩn cấp (`SoundAlarm`).
  2. Mở khóa toàn bộ cửa thoát hiểm (`UnlockAllDoors`).
  3. Cô lập hệ thống thông gió tòa nhà (`IsolateVentilation`).
- **Cầu nối Observer - Mediator:** `EmergencyBridgeObserver` hiện thực `IAlertObserver`. Khi nhận `AlertEvent` có `Severity == "CRITICAL"`, nó tự động chuyển tiếp lệnh kích hoạt khẩn cấp sang `IEmergencyMediator`. Các sự kiện mức `WARNING` hoặc `Normal` không kích hoạt quy trình này.

---

## 3. Trả lời chi tiết 4 Câu hỏi Thiết kế & Phân tích Trade-off

### Câu hỏi 1: Adapter nên kế thừa (Inheritance) hay chứa (Composition) LegacySmokeDetector?
- **Quyết định:** Sử dụng **Object Adapter (Composition)**.
- **Lý do & Đánh đổi:**
  1. **Nguyên lý thiết kế:** Tuân thủ nguyên lý vàng *"Favor object composition over class inheritance"*. Tránh liên kết tĩnh chặt chẽ vào cấu trúc bên trong của lớp cha.
  2. **Đóng gói (Encapsulation):** Nếu kế thừa (Class Adapter), lớp Adapter sẽ bộc lộ toàn bộ các phương thức công khai cũ của `LegacySmokeDetector` (như `GetSmokeLevel()`, `SetSmokeLevel()`), vi phạm nguyên tắc che giấu thông tin và làm ô nhiễm giao diện `ISensor`. Với Composition, Adapter che giấu hoàn toàn lớp cũ, chỉ công khai 2 phương thức của `ISensor`.
  3. **Khả năng thay thế & Đa hình:** Composition cho phép nạp bất kỳ đối tượng `LegacySmokeDetector` nào tại runtime, thậm chí hỗ trợ inject mock object khi kiểm thử. Kế thừa tạo ra quan hệ cứng nhắc tại compile-time.

### Câu hỏi 2: Tại sao SensorMonitor phụ thuộc vào IAlertObserver mà không trực tiếp vào DashboardObserver / EmailAlertObserver?
- **Nguyên lý Dependency Inversion (DIP):** Module cấp cao (`SensorMonitor`) không được phụ thuộc vào module cấp thấp (`DashboardObserver`, `EmailAlertObserver`). Cả hai đều phải phụ thuộc vào abstraction (`IAlertObserver`).
- **Nguyên lý Open/Closed (OCP):** Khi khuôn viên trường bổ sung thêm các kênh cảnh báo mới (như `SmsAlertObserver`, `PushNotificationObserver`), ta chỉ cần tạo lớp mới kế thừa `IAlertObserver` và đăng ký vào `SensorMonitor` mà **không cần sửa đổi một dòng code nào** trong `SensorMonitor`.
- **Khả năng kiểm thử (Testability):** Cho phép inject các mock/stub observer để kiểm tra chính xác các ngưỡng logic của `SensorMonitor` mà không gây ra tác dụng phụ (như gửi email thật).

### Câu hỏi 3: Nếu Alarm gọi trực tiếp DoorController khi có cháy thì điều gì xảy ra? Tại sao cần Mediator?
- **Nguy cơ khi liên kết trực tiếp (Mesh Topology):**
  - Còi báo động `Alarm` bị gắn chặt trách nhiệm điều khiển cửa thoát hiểm `DoorController`, vi phạm nghiêm trọng **Single Responsibility Principle (SRP)**.
  - Khi cần thêm thiết bị thứ 3, thứ 4 (`VentilationController`, `SprinklerSystem`), các thiết bị sẽ tham chiếu chéo lẫn nhau tạo thành một mạng lưới phụ thuộc xoắn ốc $O(N^2)$.
  - Rất khó kiểm thử độc lập còi báo động vì mỗi lần test `Alarm` lại kích hoạt mở cửa.
- **Vai trò của Mediator:**
  - Chuyển cấu trúc $O(N^2)$ thành cấu trúc hình sao $O(N)$.
  - Tách biệt hoàn toàn việc kích hoạt vật lý của thiết bị khỏi kịch bản vận hành an toàn. Còi và cửa hoàn toàn không biết đến sự tồn tại của nhau; toàn bộ kịch bản được tập trung tại `EmergencyMediator`.

### Câu hỏi 4: Ngưỡng cảnh báo (50%, 70%) nên được kiểm tra ở Sensor, SensorMonitor hay Observer?
- **Quyết định:** Ngưỡng cảnh báo **bắt buộc phải đặt tại `SensorMonitor`**.
- **Phân tích trách nhiệm:**
  - **Đặt tại Sensor là sai:** Cảm biến là thành phần tầng vật lý, chỉ có trách nhiệm đo đạc và phản ánh trung thực thông số môi trường (`ReadValue()`). Nếu đặt ngưỡng tại sensor, ta sẽ biến sensor thành một module chứa logic nghiệp vụ, làm mất tính tái sử dụng (ví dụ cùng một cảm biến khói có ngưỡng nhạy khác nhau giữa phòng thí nghiệm hóa chất và nhà bếp).
  - **Đặt tại Observer là sai:** Gây trùng lặp logic nghiệp vụ (DRY violation). Mỗi observer (Dashboard, Email, Log) đều phải tự viết lại các câu lệnh `if-else` phân ngưỡng. Nếu ngưỡng cảnh báo thay đổi, lập trình viên phải sửa đổi ở tất cả các observer.
  - **`SensorMonitor` là nơi duy nhất hợp lý:** Đây là đối tượng giám sát trung tâm nắm giữ toàn bộ luồng dữ liệu (Data Pipeline). Nó đóng gói nghiệp vụ phân loại mức độ rủi ro, tạo ra sự kiện nghiệp vụ `AlertEvent` mang đầy đủ ngữ cảnh để phát tán tới các observer.

---

## 4. Cấu trúc Thư mục Dự án

```
Week11/
│
├── BaoCao.tex               # Báo cáo LaTeX chi tiết
├── BaoCao.pdf               # Báo cáo PDF đã biên dịch hoàn chỉnh
├── lab w11-pattern.pdf       # Đề bài thực hành tuần 11
├── README.md                # Tài liệu hướng dẫn dự án
├── .gitignore               # Tệp cấu hình bỏ qua file build & temp
│
└── Source/                  # Mã nguồn chương trình C# .NET 10
    ├── CampusSafetyHub.csproj
    ├── Program.cs           # Kịch bản Demo mô phỏng & Bộ 8 ca kiểm thử
    │
    ├── Sensors/             # Thành phần Cảm biến & Adapter Pattern
    │   ├── ISensor.cs
    │   ├── LegacySmokeDetector.cs
    │   └── SmokeSensorAdapter.cs
    │
    ├── Models/              # Mô hình dữ liệu
    │   └── AlertEvent.cs
    │
    ├── Monitor/             # Thành phần Subject (Observer Pattern)
    │   └── SensorMonitor.cs
    │
    ├── Observers/           # Các Observer quan sát sự kiện cảnh báo
    │   ├── IAlertObserver.cs
    │   ├── DashboardObserver.cs
    │   ├── EmailAlertObserver.cs
    │   ├── AuditLogObserver.cs
    │   └── EmergencyBridgeObserver.cs   # Cầu nối sang Mediator
    │
    ├── Devices/             # Các thiết bị ngoại vi ứng phó khẩn cấp
    │   ├── IAlarm.cs / Alarm.cs
    │   ├── IDoorController.cs / DoorController.cs
    │   └── IVentilationController.cs / VentilationController.cs
    │
    └── Mediator/            # Thành phần Điều phối khẩn cấp (Mediator Pattern)
        ├── IEmergencyMediator.cs
        └── EmergencyMediator.cs
```

---

## 5. Hướng dẫn Biên dịch và Chạy thử nghiệm

### 5.1. Yêu cầu môi trường
- .NET SDK: Phiên bản 8.0, 9.0 hoặc 10.0 (Mã nguồn tương thích hoàn toàn).
- MiKTeX / TeX Live (nếu cần biên dịch lại file báo cáo LaTeX `BaoCao.tex`).

### 5.2. Chạy ứng dụng
Mở terminal tại thư mục gốc của đồ án (`D:\OOp\Week11`) hoặc thư mục `Source`:
```bash
cd Source
dotnet run
```
Hoặc chạy trực tiếp file nhị phân đã build:
```bash
./Source/bin/Debug/net10.0/CampusSafetyHub.exe
```

Chương trình sẽ tự động thực thi 2 phần:
1. **Phần 1 - Kịch bản Mô phỏng Hoạt động:** Chạy qua 4 mốc nồng độ khói ($300 \rightarrow 30\%$, $600 \rightarrow 60\%$, $850 \rightarrow 85\%$, và khôi phục về $150 \rightarrow 15\%$).
2. **Phần 2 - Bộ 8 Ca Kiểm thử Tự động (Automated Test Suite):**
   - **T1:** Kiểm tra Adapter chuẩn hóa thang đo 0..1000 sang 0..100%.
   - **T2:** Kiểm tra ngưỡng Normal (< 50%) không kích hoạt phát tán cảnh báo.
   - **T3:** Kiểm tra ngưỡng WARNING (50% - 70%) phát tán chính xác đến các Observer.
   - **T4:** Kiểm tra ngưỡng CRITICAL (>= 70%) phát tán chính xác.
   - **T5:** Kiểm tra tính năng Đăng ký (Attach) và Hủy đăng ký (Detach) của Observer.
   - **T6:** Kiểm tra tính năng Cô lập lỗi Observer (Fault Isolation).
   - **T7:** Kiểm tra Mediator điều phối đồng bộ 3 thiết bị khi có cháy.
   - **T8:** Kiểm tra kịch bản tích hợp End-to-End từ Sensor -> Monitor -> Bridge -> Mediator.

---

## 6. Hướng dẫn Đưa Ảnh Chụp Màn hình vào Báo cáo LaTeX

Báo cáo LaTeX `BaoCao.tex` đã được cấu hình với cơ chế dự phòng linh hoạt (`\IfFileExists`):
- Để thêm ảnh chụp màn hình demo, bạn chụp cửa sổ console chạy phần 1 và lưu vào thư mục gốc `D:\OOp\Week11` với tên: `demo.png`.
- Để thêm ảnh chụp màn hình kiểm thử, bạn chụp cửa sổ console chạy phần 2 và lưu vào thư mục gốc `D:\OOp\Week11` với tên: `tests.png`.
- Sau đó biên dịch lại file PDF:
  ```bash
  xelatex -interaction=nonstopmode BaoCao.tex
  ```
- Nếu chưa có ảnh, file LaTeX vẫn biên dịch ra PDF hoàn chỉnh với các khung hộp placeholder ghi chú rõ ràng mà không phát sinh lỗi.
