/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Devices;
using CampusSafetyHub.Mediator;
using CampusSafetyHub.Models;
using CampusSafetyHub.Monitor;
using CampusSafetyHub.Observers;
using CampusSafetyHub.Sensors;

namespace CampusSafetyHub;

/// <summary>
/// Chương trình thực thi chính hệ thống Campus Safety Hub.
/// Bao gồm 2 phần:
/// Phần 1: Kịch bản mô phỏng tương tác thực tế kết hợp Adapter, Observer và Mediator.
/// Phần 2: Bộ kiểm thử tự động toàn diện đối soát các yêu cầu kỹ thuật và tiêu chí chấm điểm.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.ForegroundColor = ConsoleColor.Cyan;
       
        Console.ResetColor();

        // 1. Chạy kịch bản mô phỏng trực quan
        RunLiveDemonstration();

        // 2. Chạy bộ kiểm thử tự động
        RunAutomatedTestSuite();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("================================================================================");
        Console.WriteLine("                HOÀN THÀNH CHƯƠNG TRÌNH THỰC HÀNH TUẦN 11                       ");
        Console.WriteLine("================================================================================");
        Console.ResetColor();
    }

    /// <summary>
    /// Kịch bản minh họa hoạt động thực tế của Campus Safety Hub:
    /// Tích hợp phần cứng cảm biến cũ qua Adapter, giám sát qua Observer và điều phối khẩn cấp qua Mediator.
    /// </summary>
    private static void RunLiveDemonstration()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("********************************************************************************");
        Console.WriteLine("     PHẦN 1: KỊCH BẢN MÔ PHỎNG VẬN HÀNH TRUNG TÂM GIÁM SÁT CAMPUS SAFETY HUB    ");
        Console.WriteLine("********************************************************************************\n");
        Console.ResetColor();

        // Bước 1: Khởi tạo phần cứng cũ và đóng gói qua Adapter (Adapter Pattern)
        Console.WriteLine("--- [BƯỚC 1] Khởi tạo cảm biến khói Legacy và đóng gói qua SmokeSensorAdapter ---");
        var legacyDetector = new LegacySmokeDetector(smokeLevel: 320); // 320 / 1000
        ISensor smokeSensor = new SmokeSensorAdapter(legacyDetector);
        Console.WriteLine($"[HARDWARE] Cảm biến khói cũ: Thang gốc = {legacyDetector.GetSmokeLevel()}/1000");
        Console.WriteLine($"[ADAPTER]  Chuẩn hóa ISensor: Giá trị = {smokeSensor.ReadValue()}{smokeSensor.GetUnit()}\n");

        // Bước 2: Khởi tạo các thiết bị điều phối ứng phó khẩn cấp và Mediator (Mediator Pattern)
        Console.WriteLine("--- [BƯỚC 2] Khởi tạo các thiết bị ứng phó phòng máy và EmergencyMediator ---");
        var alarm = new Alarm();
        var doorController = new DoorController();
        var ventilationController = new VentilationController();
        var mediator = new EmergencyMediator(alarm, doorController, ventilationController);
        Console.WriteLine("[SETUP] Mediator đã sẵn sàng kết nối còi báo động, cửa an toàn và quạt thông gió.\n");

        // Bước 3: Khởi tạo trung tâm giám sát và các Observer (Observer Pattern)
        Console.WriteLine("--- [BƯỚC 3] Thiết lập SensorMonitor và đăng ký các Observers ---");
        var monitor = new SensorMonitor();

        var dashboard = new DashboardObserver();
        var emailObserver = new EmailAlertObserver("it-security@campus.edu.vn");
        var auditLogger = new AuditLogObserver();
        var emergencyBridge = new EmergencyBridgeObserver(mediator);

        monitor.Attach(dashboard);
        monitor.Attach(emailObserver);
        monitor.Attach(auditLogger);
        monitor.Attach(emergencyBridge);

        // Kiểm tra tính năng chống đăng ký trùng lặp
        monitor.Attach(dashboard);
        Console.WriteLine($"[OBSERVER SETUP] Đã đăng ký 4 observers. Số lượng theo dõi thực tế: {monitor.ObserverCount}\n");

        // Bước 4: Tình huống 1 - Mức khói an toàn (Dưới 50%)
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">>> TÌNH HUỐNG 1: Mức khói ở mức bình thường (32.0%) <<<");
        Console.ResetColor();
        Console.WriteLine("SensorMonitor thực hiện kiểm tra định kỳ...");
        monitor.CheckSensor("ServerRoom-ZoneA", smokeSensor);
        Console.WriteLine("Kết quả: Khói dưới 50% -> Trạng thái an toàn, không phát sinh cảnh báo.\n");

        // Bước 5: Tình huống 2 - Mức khói tăng nhẹ lên ngưỡng CẢNH BÁO (WARNING: 50% - < 70%)
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(">>> TÌNH HUỐNG 2: Mức khói tăng nhẹ, đạt mức WARNING (62.0%) <<<");
        Console.ResetColor();
        legacyDetector.SetSmokeLevel(620); // 62.0%
        Console.WriteLine($"[HARDWARE] Cảm biến khói ghi nhận mức tăng: {legacyDetector.GetSmokeLevel()}/1000 ({smokeSensor.ReadValue()}%)\n");
        monitor.CheckSensor("ServerRoom-ZoneA", smokeSensor);
        Console.WriteLine("Nhận xét tình huống 2: Dashboard, Email và Audit Log ghi nhận cảnh báo WARNING;");
        Console.WriteLine("                       EmergencyBridge KHÔNG kích hoạt phản ứng khẩn cấp vì chưa đạt CRITICAL.\n");

        // Bước 6: Tình huống 3 - Hỏa hoạn bùng phát, đạt ngưỡng NGUY HIỂM TỘI HẠI (CRITICAL: >= 70%)
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(">>> TÌNH HUỐNG 3: HỎA HOẠN BÙNG PHÁT, ĐẠT MỨC CRITICAL (84.5%) <<<");
        Console.ResetColor();
        legacyDetector.SetSmokeLevel(845); // 84.5%
        Console.WriteLine($"[HARDWARE] Cảm biến khói ghi nhận mức tăng đột biến: {legacyDetector.GetSmokeLevel()}/1000 ({smokeSensor.ReadValue()}%)\n");
        monitor.CheckSensor("ServerRoom-ZoneA", smokeSensor);

        Console.WriteLine("Nhận xét tình huống 3:");
        Console.WriteLine("1. SensorMonitor phát hiện mức CRITICAL và thông báo tới toàn bộ Observers.");
        Console.WriteLine("2. EmergencyBridgeObserver lập tức kích hoạt sự kiện 'FIRE_DETECTED' tới EmergencyMediator.");
        Console.WriteLine("3. EmergencyMediator phối hợp đồng thời cả 3 thiết bị ứng phó:");
        Console.WriteLine($"   - Còi báo động đang hú: {alarm.IsSounding}");
        Console.WriteLine($"   - Cửa thoát hiểm đã mở khóa: {doorController.AreDoorsUnlocked}");
        Console.WriteLine($"   - Hệ thống thông gió đã cách ly và hút khói: {ventilationController.IsIsolated}\n");

        // Tình huống 4: Khôi phục hiện trường an toàn sau sự cố
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(">>> TÌNH HUỐNG 4: Xử lý xong sự cố, khôi phục hệ thống về bình thường <<<");
        Console.ResetColor();
        mediator.ResetNormal();
        legacyDetector.SetSmokeLevel(200); // Khói về 20%
        Console.WriteLine($"[HARDWARE] Khói tan về mức an toàn: {smokeSensor.ReadValue()}{smokeSensor.GetUnit()}\n");
    }

    /// <summary>
    /// Bộ kiểm thử tự động toàn diện kiểm tra tất cả các yêu cầu kỹ thuật và tiêu chí chấm điểm (T1 - T8).
    /// </summary>
    private static void RunAutomatedTestSuite()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("********************************************************************************");
        Console.WriteLine("       PHẦN 2: BỘ KIỂM THỬ TỰ ĐỘNG CHUẨN ĐÁNH GIÁ TIÊU CHÍ (T1 - T8)             ");
        Console.WriteLine("********************************************************************************\n");
        Console.ResetColor();

        int passed = 0;
        int total = 8;

        // Test 1: Kiểm thử Adapter - Chuyển đổi thang đo và trả về đúng đơn vị
        passed += AssertTest(
            id: "T1",
            name: "Adapter: Quy đổi giá trị 0..1000 sang 0..100% và đơn vị chuẩn '% smoke'",
            testAction: () =>
            {
                var legacy = new LegacySmokeDetector(735);
                ISensor adapter = new SmokeSensorAdapter(legacy);

                bool valueCorrect = Math.Abs(adapter.ReadValue() - 73.5) < 0.001;
                bool unitCorrect = adapter.GetUnit() == "% smoke";

                // Kiểm tra không sao chép trạng thái (thay đổi legacy thì adapter đọc giá trị mới)
                legacy.SetSmokeLevel(920);
                bool dynamicUpdate = Math.Abs(adapter.ReadValue() - 92.0) < 0.001;

                return valueCorrect && unitCorrect && dynamicUpdate;
            });

        // Test 2: Kiểm thử Ngưỡng an toàn - Dưới 50% không phát cảnh báo
        passed += AssertTest(
            id: "T2",
            name: "Threshold: Mức khói < 50% là bình thường, SensorMonitor không phát sự kiện",
            testAction: () =>
            {
                var monitor = new SensorMonitor();
                var dash = new DashboardObserver();
                monitor.Attach(dash);

                var legacy = new LegacySmokeDetector(499); // 49.9%
                var sensor = new SmokeSensorAdapter(legacy);

                monitor.CheckSensor("ZoneTest", sensor);

                return dash.DisplayCount == 0 && monitor.LastAlert == null;
            });

        // Test 3: Kiểm thử Ngưỡng cảnh báo WARNING (50% <= khói < 70%)
        passed += AssertTest(
            id: "T3",
            name: "Threshold: Mức khói từ 50% đến dưới 70% phát mức WARNING",
            testAction: () =>
            {
                var monitor = new SensorMonitor();
                var dash = new DashboardObserver();
                monitor.Attach(dash);

                var legacy = new LegacySmokeDetector(550); // 55.0%
                var sensor = new SmokeSensorAdapter(legacy);

                monitor.CheckSensor("ZoneTest", sensor);

                return dash.DisplayCount == 1 &&
                       monitor.LastAlert?.Severity == "WARNING" &&
                       Math.Abs(monitor.LastAlert.Value - 55.0) < 0.001;
            });

        // Test 4: Kiểm thử Ngưỡng nguy cấp CRITICAL (khói >= 70%)
        passed += AssertTest(
            id: "T4",
            name: "Threshold: Mức khói >= 70% phát mức CRITICAL",
            testAction: () =>
            {
                var monitor = new SensorMonitor();
                var dash = new DashboardObserver();
                monitor.Attach(dash);

                var legacy = new LegacySmokeDetector(700); // 70.0%
                var sensor = new SmokeSensorAdapter(legacy);

                monitor.CheckSensor("ZoneTest", sensor);

                return dash.DisplayCount == 1 &&
                       monitor.LastAlert?.Severity == "CRITICAL" &&
                       Math.Abs(monitor.LastAlert.Value - 70.0) < 0.001;
            });

        // Test 5: Kiểm thử Observer - Ngăn chặn đăng ký trùng lặp và hỗ trợ hủy đăng ký
        passed += AssertTest(
            id: "T5",
            name: "Observer Abstraction: Ngăn chặn trùng lặp khi Attach và hỗ trợ Detach",
            testAction: () =>
            {
                var monitor = new SensorMonitor();
                var obs1 = new DashboardObserver();
                var obs2 = new AuditLogObserver();

                monitor.Attach(obs1);
                monitor.Attach(obs1); // Đăng ký lại lần 2
                monitor.Attach(obs2);

                bool duplicatePrevented = monitor.ObserverCount == 2;

                monitor.Detach(obs1);
                bool detachSuccessful = monitor.ObserverCount == 1 && !monitor.Observers.Contains(obs1);

                return duplicatePrevented && detachSuccessful;
            });

        // Test 6: Kiểm thử Observer - Cách ly ngoại lệ (Fault Isolation)
        passed += AssertTest(
            id: "T6",
            name: "Observer Fault Isolation: Observer bị lỗi không làm ảnh hưởng các Observer khác",
            testAction: () =>
            {
                var monitor = new SensorMonitor();

                // Tạo một faulty observer cố tình ném ngoại lệ
                var faultyObserver = new FaultyObserver();
                var goodObserver = new DashboardObserver();

                monitor.Attach(faultyObserver);
                monitor.Attach(goodObserver);

                var legacy = new LegacySmokeDetector(800);
                var sensor = new SmokeSensorAdapter(legacy);

                // CheckSensor không được làm sập ứng dụng dù faultyObserver ném ngoại lệ
                monitor.CheckSensor("ZoneFaultTest", sensor);

                // goodObserver vẫn phải nhận được thông báo bình thường
                return goodObserver.DisplayCount == 1;
            });

        // Test 7: Kiểm thử Mediator - Phối hợp đồng bộ cả 3 thiết bị khi nhận sự kiện khẩn cấp
        passed += AssertTest(
            id: "T7",
            name: "Mediator: Điều phối đồng bộ Alarm hú, Door mở khóa, Ventilation cách ly",
            testAction: () =>
            {
                var alarm = new Alarm();
                var door = new DoorController();
                var vent = new VentilationController();
                var mediator = new EmergencyMediator(alarm, door, vent);

                var alert = new AlertEvent("ServerRoom", 85.0, "% smoke", "CRITICAL", DateTime.Now);
                mediator.TriggerEmergency("FIRE_DETECTED", alert);

                bool allCoordinated = alarm.IsSounding &&
                                     door.AreDoorsUnlocked &&
                                     vent.IsIsolated &&
                                     mediator.EmergencyActive;

                mediator.ResetNormal();
                bool resetSuccessful = !alarm.IsSounding &&
                                      !door.AreDoorsUnlocked &&
                                      !vent.IsIsolated &&
                                      !mediator.EmergencyActive;

                return allCoordinated && resetSuccessful;
            });

        // Test 8: Kiểm thử tích hợp End-to-End: Khói CRITICAL tự động kích hoạt Mediator qua Bridge
        passed += AssertTest(
            id: "T8",
            name: "E2E Integration: Cảm biến đạt CRITICAL -> Bridge kích hoạt Mediator phản ứng",
            testAction: () =>
            {
                var alarm = new Alarm();
                var door = new DoorController();
                var vent = new VentilationController();
                var mediator = new EmergencyMediator(alarm, door, vent);

                var monitor = new SensorMonitor();
                var bridge = new EmergencyBridgeObserver(mediator);
                monitor.Attach(bridge);

                var legacy = new LegacySmokeDetector(650); // 65% - WARNING
                var sensor = new SmokeSensorAdapter(legacy);

                // Bước 1: Khi WARNING, mediator KHÔNG được kích hoạt
                monitor.CheckSensor("ZoneE2E", sensor);
                bool warningIgnoredByBridge = !mediator.EmergencyActive && bridge.EmergencyTriggeredCount == 0;

                // Bước 2: Khi tăng lên CRITICAL (>= 70%), bridge kích hoạt mediator ngay lập tức
                legacy.SetSmokeLevel(760); // 76% - CRITICAL
                monitor.CheckSensor("ZoneE2E", sensor);

                bool criticalTriggered = mediator.EmergencyActive &&
                                         bridge.EmergencyTriggeredCount == 1 &&
                                         alarm.IsSounding &&
                                         door.AreDoorsUnlocked &&
                                         vent.IsIsolated;

                mediator.ResetNormal();
                return warningIgnoredByBridge && criticalTriggered;
            });

        Console.ForegroundColor = passed == total ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"\n>>> KẾT QUẢ TỔNG HỢP KIỂM THỬ: {passed}/{total} TEST CASES ĐẠT YÊU CẦU (PASS) <<<\n");
        Console.ResetColor();
    }

    /// <summary>
    /// Phương thức hỗ trợ đánh giá một ca kiểm thử, in định dạng kết quả màu sắc trực quan ra màn hình Console.
    /// </summary>
    private static int AssertTest(string id, string name, Func<bool> testAction)
    {
        Console.Write($"[{id}] {name,-72} : ");
        try
        {
            bool result = testAction();
            if (result)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("PASS");
                Console.ResetColor();
                return 1;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FAIL (Không thỏa mãn điều kiện mong đợi)");
                Console.ResetColor();
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FAIL EXCEPTION ({ex.Message})");
            Console.ResetColor();
            return 0;
        }
    }

    /// <summary>
    /// Mock Observer lỗi phục vụ ca kiểm thử T6 (Kiểm tra cơ chế Fault Isolation của Observer Pattern).
    /// </summary>
    private sealed class FaultyObserver : IAlertObserver
    {
        public void Update(AlertEvent alertEvent)
        {
            throw new InvalidOperationException("Mô phỏng lỗi đường truyền mạng hoặc ngoại lệ tại Observer!");
        }
    }
}
