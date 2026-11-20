/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Models;

namespace CampusSafetyHub.Observers;

/// <summary>
/// Giao diện Observer (Observer Interface) đại diện cho hợp đồng nhận cảnh báo.
/// Mọi đối tượng muốn lắng nghe thông báo sự kiện an toàn đều phải triển khai giao diện này.
/// </summary>
public interface IAlertObserver
{
    /// <summary>
    /// Phương thức tiếp nhận và xử lý sự kiện cảnh báo được phát đi từ SensorMonitor (Subject).
    /// </summary>
    /// <param name="alertEvent">Đối tượng sự kiện cảnh báo bất biến.</param>
    void Update(AlertEvent alertEvent);
}
