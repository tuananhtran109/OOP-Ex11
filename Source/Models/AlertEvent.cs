/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Models;

/// <summary>
/// Đối tượng dữ liệu bất biến (Immutable Data Object) mô tả sự kiện cảnh báo an toàn.
/// Được biểu diễn dưới dạng C# Record để đảm bảo tính toàn vẹn dữ liệu,
/// ngăn chặn các Observer can thiệp hoặc sửa đổi thông tin cảnh báo.
/// </summary>
/// <param name="SensorName">Tên định danh cảm biến phát hiện sự kiện.</param>
/// <param name="Value">Giá trị đo lường đã được chuẩn hóa.</param>
/// <param name="Unit">Đơn vị đo lường (ví dụ: "% smoke").</param>
/// <param name="Severity">Mức độ nghiêm trọng ("WARNING" hoặc "CRITICAL").</param>
/// <param name="OccurredAt">Thời điểm phát sinh sự kiện cảnh báo.</param>
public sealed record AlertEvent(
    string SensorName,
    double Value,
    string Unit,
    string Severity,
    DateTime OccurredAt);
