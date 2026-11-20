/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Devices;

/// <summary>
/// Giao diện điều khiển hệ thống cửa và lối thoát hiểm phòng máy.
/// </summary>
public interface IDoorController
{
    /// <summary>
    /// Trạng thái toàn bộ cửa có đang ở chế độ mở khóa thoát hiểm hay không.
    /// </summary>
    bool AreDoorsUnlocked { get; }

    /// <summary>
    /// Mở khóa khẩn cấp toàn bộ các cửa thoát nạn để người bên trong sơ tán an toàn.
    /// </summary>
    void UnlockAllDoors();

    /// <summary>
    /// Khóa lại hệ thống cửa, khôi phục chế độ bảo mật thông thường.
    /// </summary>
    void LockAllDoors();
}
