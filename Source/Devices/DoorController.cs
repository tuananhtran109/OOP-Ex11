/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Devices;

/// <summary>
/// Bộ điều khiển hệ thống cửa phòng máy.
/// Tuân thủ nguyên lý Single Responsibility, chỉ quản lý đóng/mở cửa mà không cần biết đến còi hay thông gió.
/// </summary>
public sealed class DoorController : IDoorController
{
    public bool AreDoorsUnlocked { get; private set; }

    public void UnlockAllDoors()
    {
        AreDoorsUnlocked = true;
        Console.WriteLine("[DOOR] 🚪 MỞ KHÓA TẤT CẢ CÁC CỬA THOÁT HIỂM để nhân viên sơ tán!");
    }

    public void LockAllDoors()
    {
        AreDoorsUnlocked = false;
        Console.WriteLine("[DOOR] Khóa các cửa phòng máy, kích hoạt lại hệ thống bảo vệ ra vào.");
    }
}
