/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Devices;

/// <summary>
/// Bộ điều khiển hệ thống thông gió phòng máy.
/// </summary>
public sealed class VentilationController : IVentilationController
{
    public bool IsIsolated { get; private set; }

    public void IsolateVentilation()
    {
        IsIsolated = true;
        Console.WriteLine("[VENTILATION] 🌀 CÁCH LY HỆ THỐNG THÔNG GIÓ: Ngắt luồng cấp khí tươi và bật quạt hút khói sự cố!");
    }

    public void ResumeNormal()
    {
        IsIsolated = false;
        Console.WriteLine("[VENTILATION] Khôi phục vận hành hệ thống quạt thông gió bình thường.");
    }
}
