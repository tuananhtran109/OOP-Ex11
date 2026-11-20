/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Sensors;

/// <summary>
/// Lớp thiết bị cảm biến khói đời cũ (Adaptee).
/// Lớp này có sẵn từ hệ thống cũ, giả định không được phép sửa đổi mã nguồn.
/// Cung cấp dữ liệu theo thang đo số nguyên từ 0 đến 1000.
/// </summary>
public sealed class LegacySmokeDetector
{
    private int smokeLevel;

    /// <summary>
    /// Khởi tạo thiết bị cảm biến khói cũ với mức khói ban đầu.
    /// </summary>
    /// <param name="smokeLevel">Giá trị khói từ 0 đến 1000.</param>
    public LegacySmokeDetector(int smokeLevel)
    {
        this.smokeLevel = Math.Clamp(smokeLevel, 0, 1000);
    }

    /// <summary>
    /// Lấy mức độ khói hiện tại theo thang đo cũ 0..1000.
    /// </summary>
    /// <returns>Mức độ khói nguyên từ 0 đến 1000.</returns>
    public int GetSmokeLevel() => smokeLevel;

    /// <summary>
    /// Lấy thông tin mô tả thiết bị cũ.
    /// </summary>
    /// <returns>Chuỗi mô tả phần cứng cũ.</returns>
    public string GetDescription() => "Legacy smoke detector";

    /// <summary>
    /// Cập nhật mức độ khói của thiết bị (mô phỏng thay đổi môi trường).
    /// </summary>
    /// <param name="value">Giá trị khói mới.</param>
    public void SetSmokeLevel(int value)
    {
        smokeLevel = Math.Clamp(value, 0, 1000);
    }
}
