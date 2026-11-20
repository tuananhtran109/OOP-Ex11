/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Devices;

/// <summary>
/// Thiết bị còi báo động khẩn cấp trong phòng máy.
/// Hoạt động độc lập, không tham chiếu trực tiếp đến cửa hay quạt thông gió.
/// </summary>
public sealed class Alarm : IAlarm
{
    public bool IsSounding { get; private set; }

    public void SoundAlarm(string reason)
    {
        IsSounding = true;
        Console.WriteLine($"[ALARM] 🚨 CÒI BÁO ĐỘNG BẬT! Lý do: {reason}");
    }

    public void Silence()
    {
        IsSounding = false;
        Console.WriteLine("[ALARM] Còi báo động đã tắt, hệ thống an toàn.");
    }
}
