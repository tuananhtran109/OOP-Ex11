/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Devices;

/// <summary>
/// Giao diện điều khiển hệ thống thông gió và điều hòa khí phòng máy.
/// </summary>
public interface IVentilationController
{
    /// <summary>
    /// Trạng thái hệ thống thông gió có đang ở chế độ cách ly/hút khói khẩn cấp hay không.
    /// </summary>
    bool IsIsolated { get; }

    /// <summary>
    /// Kích hoạt chế độ cách ly: ngắt quạt cấp dưỡng khí và bật hệ thống hút khói sự cố.
    /// </summary>
    void IsolateVentilation();

    /// <summary>
    /// Khôi phục hệ thống thông gió và lưu thông khí về trạng thái bình thường.
    /// </summary>
    void ResumeNormal();
}
