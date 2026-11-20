/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Mediator;
using CampusSafetyHub.Models;

namespace CampusSafetyHub.Observers;

/// <summary>
/// Observer đóng vai trò cầu nối (Bridge) giữa hệ thống giám sát cảnh báo (Observer Pattern)
/// và hệ thống điều phối thiết bị ứng phó khẩn cấp (Mediator Pattern).
/// Lớp này lắng nghe các AlertEvent từ SensorMonitor; khi phát hiện mức độ nghiêm trọng là "CRITICAL",
/// nó lập tức kích hoạt sự kiện "FIRE_DETECTED" sang EmergencyMediator.
/// </summary>
public sealed class EmergencyBridgeObserver : IAlertObserver
{
    private readonly IEmergencyMediator mediator;

    /// <summary>
    /// Số lần đã kích hoạt điều phối khẩn cấp qua Mediator.
    /// </summary>
    public int EmergencyTriggeredCount { get; private set; }

    /// <summary>
    /// Cảnh báo nghiêm trọng gần nhất đã kích hoạt cầu nối khẩn cấp.
    /// </summary>
    public AlertEvent? LastTriggeredAlert { get; private set; }

    /// <summary>
    /// Khởi tạo cầu nối với tham chiếu trừu tượng tới IEmergencyMediator.
    /// </summary>
    /// <param name="mediator">Bộ điều phối khẩn cấp.</param>
    /// <exception cref="ArgumentNullException">Ném ra khi mediator bị null.</exception>
    public EmergencyBridgeObserver(IEmergencyMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Tiếp nhận cảnh báo:
    /// - Nếu mức độ là "CRITICAL": Lập tức kích hoạt sự kiện "FIRE_DETECTED" sang Mediator.
    /// - Nếu mức độ là "WARNING": Chỉ ghi nhận thông tin, chưa kích hoạt ứng phó toàn diện.
    /// </summary>
    /// <param name="alertEvent">Sự kiện cảnh báo an toàn.</param>
    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        if (string.Equals(alertEvent.Severity, "CRITICAL", StringComparison.OrdinalIgnoreCase))
        {
            EmergencyTriggeredCount++;
            LastTriggeredAlert = alertEvent;

            Console.WriteLine(
                $"[BRIDGE] ⚠️ CẢNH BÁO NGUY CẤP CRITICAL từ '{alertEvent.SensorName}' ({alertEvent.Value:F1}{alertEvent.Unit})! " +
                $"Kích hoạt sự kiện 'FIRE_DETECTED' sang EmergencyMediator...");

            mediator.TriggerEmergency("FIRE_DETECTED", alertEvent);
        }
        else
        {
            Console.WriteLine(
                $"[BRIDGE] ℹ️ Nhận cảnh báo mức {alertEvent.Severity} từ '{alertEvent.SensorName}'. " +
                "Chưa đạt ngưỡng kích hoạt điều phối khẩn cấp (yêu cầu mức CRITICAL >= 70%).");
        }
    }
}
