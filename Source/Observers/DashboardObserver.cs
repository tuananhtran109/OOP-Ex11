/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Models;

namespace CampusSafetyHub.Observers;

/// <summary>
/// Observer hiển thị thông tin cảnh báo trực tiếp lên bảng điều khiển Dashboard trung tâm.
/// </summary>
public sealed class DashboardObserver : IAlertObserver
{
    /// <summary>
    /// Số lượng cảnh báo đã hiển thị trên Dashboard.
    /// </summary>
    public int DisplayCount { get; private set; }

    /// <summary>
    /// Cảnh báo gần nhất được hiển thị trên Dashboard.
    /// </summary>
    public AlertEvent? LastAlert { get; private set; }

    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        DisplayCount++;
        LastAlert = alertEvent;

        Console.WriteLine(
            $"[DASHBOARD] {alertEvent.SensorName}: " +
            $"{alertEvent.Value:F1}{alertEvent.Unit} " +
            $"[{alertEvent.Severity}]");
    }
}
