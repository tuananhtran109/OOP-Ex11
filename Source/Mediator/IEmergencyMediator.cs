/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Models;

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Giao diện điều phối viên khẩn cấp (Mediator Interface).
/// Đóng vai trò là trung tâm phối hợp hoạt động của các thiết bị phần cứng (còi, cửa, thông gió)
/// theo các sự kiện an toàn, giúp các thiết bị không cần biết hay phụ thuộc trực tiếp vào nhau.
/// </summary>
public interface IEmergencyMediator
{
    /// <summary>
    /// Kích hoạt quy trình phản ứng khẩn cấp dựa trên loại sự kiện an toàn.
    /// </summary>
    /// <param name="eventType">Mã loại sự kiện (ví dụ: "FIRE_DETECTED").</param>
    /// <param name="alertEvent">Thông tin chi tiết về sự kiện cảnh báo (tùy chọn).</param>
    void TriggerEmergency(string eventType, AlertEvent? alertEvent = null);

    /// <summary>
    /// Khôi phục toàn bộ các thiết bị về trạng thái hoạt động an toàn ban đầu.
    /// </summary>
    void ResetNormal();
}
