/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Models;
using CampusSafetyHub.Observers;
using CampusSafetyHub.Sensors;

namespace CampusSafetyHub.Monitor;

/// <summary>
/// Lớp trung tâm giám sát cảm biến (Subject trong mẫu thiết kế Observer).
/// Chịu trách nhiệm:
/// 1. Quản lý danh sách người quan sát (Attach / Detach) đảm bảo không đăng ký trùng lặp.
/// 2. Đọc dữ liệu từ cảm biến thông qua giao diện ISensor.
/// 3. So khớp ngưỡng an toàn nghiệp vụ (dưới 50%: Bình thường, 50% - dưới 70%: WARNING, từ 70%: CRITICAL).
/// 4. Đóng gói và phát tán sự kiện cảnh báo AlertEvent an toàn tới các Observer.
/// </summary>
public sealed class SensorMonitor
{
    private readonly List<IAlertObserver> observers = new();

    /// <summary>
    /// Số lượng Observer hiện đang đăng ký lắng nghe trong hệ thống.
    /// </summary>
    public int ObserverCount => observers.Count;

    /// <summary>
    /// Danh sách chỉ đọc các Observer đang được theo dõi (phục vụ kiểm thử).
    /// </summary>
    public IReadOnlyList<IAlertObserver> Observers => observers.AsReadOnly();

    /// <summary>
    /// Lưu trữ sự kiện cảnh báo gần nhất được phát đi (hỗ trợ kiểm thử tự động).
    /// </summary>
    public AlertEvent? LastAlert { get; private set; }

    /// <summary>
    /// Đăng ký thêm một Observer vào danh sách theo dõi.
    /// Đảm bảo không cho phép cùng một Observer đăng ký 2 lần.
    /// </summary>
    /// <param name="observer">Observer muốn đăng ký.</param>
    /// <exception cref="ArgumentNullException">Ném ra khi tham chiếu observer bị null.</exception>
    public void Attach(IAlertObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    /// <summary>
    /// Hủy đăng ký một Observer khỏi danh sách theo dõi.
    /// </summary>
    /// <param name="observer">Observer muốn hủy theo dõi.</param>
    public void Detach(IAlertObserver observer)
    {
        if (observer is not null)
        {
            observers.Remove(observer);
        }
    }

    /// <summary>
    /// Đọc giá trị cảm biến, đánh giá quy tắc ngưỡng an toàn và phát cảnh báo nếu vượt ngưỡng.
    /// - Dưới 50%: Bình thường, không phát cảnh báo.
    /// - Từ 50% đến dưới 70%: Mức WARNING.
    /// - Từ 70% trở lên: Mức CRITICAL.
    /// </summary>
    /// <param name="sensorName">Tên định danh cảm biến.</param>
    /// <param name="sensor">Đối tượng cảm biến triển khai ISensor.</param>
    public void CheckSensor(string sensorName, ISensor sensor)
    {
        ArgumentNullException.ThrowIfNull(sensor);

        double value = sensor.ReadValue();
        string? severity = value switch
        {
            >= 70.0 => "CRITICAL",
            >= 50.0 => "WARNING",
            _ => null
        };

        if (severity is null)
        {
            return;
        }

        var alertEvent = new AlertEvent(
            sensorName,
            value,
            sensor.GetUnit(),
            severity,
            DateTime.Now);

        LastAlert = alertEvent;
        Notify(alertEvent);
    }

    /// <summary>
    /// Phát thông báo tới toàn bộ Observer đã đăng ký.
    /// - Duyệt trên bản sao ToArray() để tránh lỗi collection bị thay đổi khi observer tự gỡ bỏ trong lúc thông báo.
    /// - Bọc try-catch độc lập cho từng Observer để đảm bảo lỗi của một Observer không làm gián đoạn các Observer còn lại.
    /// </summary>
    /// <param name="alertEvent">Đối tượng sự kiện cảnh báo.</param>
    private void Notify(AlertEvent alertEvent)
    {
        foreach (IAlertObserver observer in observers.ToArray())
        {
            try
            {
                observer.Update(alertEvent);
            }
            catch (Exception ex)
            {
                // Yêu cầu 5: Một observer lỗi không được làm sai dữ liệu của observer khác.
                Console.WriteLine($"[MONITOR ERROR] Ngoại lệ xảy ra tại Observer '{observer.GetType().Name}': {ex.Message}");
            }
        }
    }
}
