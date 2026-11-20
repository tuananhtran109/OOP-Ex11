/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

namespace CampusSafetyHub.Sensors;

/// <summary>
/// Lớp chuyển đổi (Adapter) cho cảm biến khói cũ.
/// Áp dụng mẫu thiết kế Adapter (dạng Object Adapter) để giúp LegacySmokeDetector
/// tương thích hoàn toàn với giao diện ISensor của hệ thống mới.
/// </summary>
public sealed class SmokeSensorAdapter : ISensor
{
    private readonly LegacySmokeDetector legacy;

    /// <summary>
    /// Khởi tạo Adapter thông qua cơ chế Composition (chứa đối tượng cũ).
    /// </summary>
    /// <param name="legacy">Thể hiện cảm biến khói đời cũ.</param>
    /// <exception cref="ArgumentNullException">Ném ra khi tham chiếu legacy bị null.</exception>
    public SmokeSensorAdapter(LegacySmokeDetector legacy)
    {
        this.legacy = legacy ?? throw new ArgumentNullException(nameof(legacy));
    }

    /// <summary>
    /// Đọc giá trị đo từ thiết bị cũ và quy đổi từ thang 0..1000 sang thang phần trăm 0..100.
    /// Không sao chép trạng thái nội bộ mà truy xuất trực tiếp trạng thái hiện thời của thiết bị cũ.
    /// </summary>
    /// <returns>Tỷ lệ phần trăm khói (0.0 đến 100.0).</returns>
    public double ReadValue()
    {
        return legacy.GetSmokeLevel() / 10.0;
    }

    /// <summary>
    /// Trả về đơn vị đo lường chuẩn hóa của cảm biến khói.
    /// </summary>
    /// <returns>Chuỗi "% smoke".</returns>
    public string GetUnit() => "% smoke";
}
