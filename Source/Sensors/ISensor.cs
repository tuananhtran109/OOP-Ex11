/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Sensors;

/// <summary>
/// Giao diện chuẩn (Target Interface) mà hệ thống giám sát an toàn sử dụng để đọc giá trị từ các cảm biến.
/// </summary>
public interface ISensor
{
    /// <summary>
    /// Đọc giá trị đo lường hiện tại của cảm biến theo thang chuẩn.
    /// </summary>
    /// <returns>Giá trị cảm biến dạng số thực.</returns>
    double ReadValue();

    /// <summary>
    /// Lấy đơn vị đo lường tương ứng của cảm biến (ví dụ: "% smoke", "°C").
    /// </summary>
    /// <returns>Chuỗi định danh đơn vị.</returns>
    string GetUnit();
}
