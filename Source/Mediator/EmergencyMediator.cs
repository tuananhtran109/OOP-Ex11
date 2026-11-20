/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Devices;
using CampusSafetyHub.Models;

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Lớp điều phối ứng phó khẩn cấp (Concrete Mediator).
/// Đóng vai trò là trung tâm giao tiếp duy nhất giữa còi báo động (Alarm),
/// bộ điều khiển cửa (DoorController) và hệ thống thông gió (VentilationController).
/// Loại bỏ hoàn toàn sự phụ thuộc chéo nhiều-nhiều (many-to-many) giữa các thiết bị.
/// </summary>
public sealed class EmergencyMediator : IEmergencyMediator
{
    private readonly IAlarm alarm;
    private readonly IDoorController doorController;
    private readonly IVentilationController ventilationController;

    /// <summary>
    /// Cho biết hệ thống hiện có đang ở trạng thái kích hoạt quy trình ứng phó khẩn cấp hay không.
    /// </summary>
    public bool EmergencyActive { get; private set; }

    /// <summary>
    /// Loại sự kiện khẩn cấp gần nhất đã được kích hoạt.
    /// </summary>
    public string? LastEventType { get; private set; }

    /// <summary>
    /// Khởi tạo Mediator với các thiết bị thành viên (Colleagues).
    /// </summary>
    /// <param name="alarm">Hệ thống còi báo động.</param>
    /// <param name="doorController">Hệ thống cửa thoát hiểm.</param>
    /// <param name="ventilationController">Hệ thống quạt thông gió.</param>
    public EmergencyMediator(
        IAlarm alarm,
        IDoorController doorController,
        IVentilationController ventilationController)
    {
        this.alarm = alarm ?? throw new ArgumentNullException(nameof(alarm));
        this.doorController = doorController ?? throw new ArgumentNullException(nameof(doorController));
        this.ventilationController = ventilationController ?? throw new ArgumentNullException(nameof(ventilationController));
    }

    /// <summary>
    /// Tiếp nhận sự kiện và phối hợp hành vi đồng bộ giữa các thiết bị:
    /// Khi phát hiện hỏa hoạn (FIRE_DETECTED):
    /// 1. Còi báo động hú to cảnh báo nguy hiểm.
    /// 2. Cửa phòng tự động mở khóa để nhân sự thoát nạn.
    /// 3. Hệ thống thông gió lập tức cách ly, chuyển sang chế độ hút khói ngăn đám cháy lan rộng.
    /// </summary>
    /// <param name="eventType">Mã loại sự kiện.</param>
    /// <param name="alertEvent">Thông tin chi tiết cảnh báo.</param>
    public void TriggerEmergency(string eventType, AlertEvent? alertEvent = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);

        LastEventType = eventType;
        EmergencyActive = true;

        Console.WriteLine($"\n[MEDIATOR] >>> BẮT ĐẦU ĐIỀU PHỐI KHẨN CẤP CHO SỰ KIỆN: {eventType} <<<");

        if (eventType.Equals("FIRE_DETECTED", StringComparison.OrdinalIgnoreCase))
        {
            string reason = alertEvent is not null
                ? $"Phát hiện cháy từ {alertEvent.SensorName} ({alertEvent.Value:F1}{alertEvent.Unit})"
                : "Phát hiện cháy phòng máy!";

            // 1. Phối hợp Alarm
            alarm.SoundAlarm(reason);

            // 2. Phối hợp DoorController
            doorController.UnlockAllDoors();

            // 3. Phối hợp VentilationController
            ventilationController.IsolateVentilation();

            Console.WriteLine("[MEDIATOR] >>> Hoàn thành điều phối phản ứng khẩn cấp cho tất cả thiết bị. <<<\n");
        }
        else
        {
            Console.WriteLine($"[MEDIATOR] Sự kiện '{eventType}' chưa có kịch bản xử lý tự động.");
        }
    }

    /// <summary>
    /// Khôi phục toàn bộ các thiết bị về trạng thái an toàn bình thường.
    /// </summary>
    public void ResetNormal()
    {
        EmergencyActive = false;
        LastEventType = null;

        Console.WriteLine("\n[MEDIATOR] >>> KHÔI PHỤC TOÀN BỘ THIẾT BỊ VỀ CHẾ ĐỘ HOẠT ĐỘNG BÌNH THƯỜNG <<<");
        alarm.Silence();
        doorController.LockAllDoors();
        ventilationController.ResumeNormal();
        Console.WriteLine("[MEDIATOR] >>> Hệ thống phòng máy đã trở lại trạng thái an toàn. <<<\n");
    }
}
