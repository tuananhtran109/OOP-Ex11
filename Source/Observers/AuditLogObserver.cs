/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Models;

namespace CampusSafetyHub.Observers;

/// <summary>
/// Observer ghi chép nhật ký kiểm toán (Audit Trail) phục vụ công tác giám định và hậu kiểm an toàn.
/// Duy trì danh sách sự kiện bất biến trong bộ nhớ.
/// </summary>
public sealed class AuditLogObserver : IAlertObserver
{
    private readonly List<AlertEvent> auditHistory = new();

    /// <summary>
    /// Danh sách lịch sử các sự kiện an toàn đã ghi nhận (chỉ đọc).
    /// </summary>
    public IReadOnlyList<AlertEvent> AuditHistory => auditHistory.AsReadOnly();

    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        auditHistory.Add(alertEvent);

        Console.WriteLine(
            $"[AUDIT LOG] 📝 [{alertEvent.OccurredAt:yyyy-MM-dd HH:mm:ss}] " +
            $"Ghi nhận nhật ký an toàn: Thiết bị='{alertEvent.SensorName}', " +
            $"Mức độ='{alertEvent.Severity}', Giá trị={alertEvent.Value:F1}{alertEvent.Unit}");
    }
}
