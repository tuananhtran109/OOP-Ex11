/****************/
// MSSV:202418834
// Họ tên: Trần Tuấn Anh
/****************/

using CampusSafetyHub.Models;

namespace CampusSafetyHub.Observers;

/// <summary>
/// Observer mô phỏng việc gửi email thông báo khẩn cấp tới Ban Quản Trị và Đội Kỹ Thuật.
/// </summary>
public sealed class EmailAlertObserver : IAlertObserver
{
    private readonly string recipientEmail;

    /// <summary>
    /// Số lượng email cảnh báo đã gửi.
    /// </summary>
    public int EmailsSentCount { get; private set; }

    /// <summary>
    /// Nội dung cảnh báo gần nhất đã được gửi đi.
    /// </summary>
    public AlertEvent? LastEmailAlert { get; private set; }

    /// <summary>
    /// Khởi tạo EmailAlertObserver với địa chỉ email tiếp nhận.
    /// </summary>
    /// <param name="recipientEmail">Email người nhận cảnh báo.</param>
    public EmailAlertObserver(string recipientEmail = "admin-campus@safetyhub.edu.vn")
    {
        this.recipientEmail = recipientEmail;
    }

    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        EmailsSentCount++;
        LastEmailAlert = alertEvent;

        Console.WriteLine(
            $"[EMAIL] 📧 Gửi cảnh báo tới <{recipientEmail}>: " +
            $"Cảm biến '{alertEvent.SensorName}' ghi nhận mức {alertEvent.Severity} " +
            $"({alertEvent.Value:F1}{alertEvent.Unit}) vào lúc {alertEvent.OccurredAt:HH:mm:ss}");
    }
}
