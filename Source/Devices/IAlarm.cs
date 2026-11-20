/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Devices;

/// <summary>
/// Giao diện điều khiển hệ thống còi báo động khẩn cấp.
/// </summary>
public interface IAlarm
{
    /// <summary>
    /// Trạng thái còi báo động có đang hú hay không.
    /// </summary>
    bool IsSounding { get; }

    /// <summary>
    /// Kích hoạt còi báo động khẩn cấp với thông điệp lý do.
    /// </summary>
    /// <param name="reason">Lý do kích hoạt còi.</param>
    void SoundAlarm(string reason);

    /// <summary>
    /// Tắt còi báo động, trở về trạng thái bình thường.
    /// </summary>
    void Silence();
}
