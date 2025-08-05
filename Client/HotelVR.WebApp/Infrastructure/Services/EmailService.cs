using System;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using HotelVR.WebApp.Infrastructure.Services.Interfaceses;

namespace HotelVR.WebApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public EmailService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task SendEmailAsync(string subject, string body, string toEmail)
        {
            var request = new
            {
                subject,
                body,
                toEmail
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/email", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Email gönderme hatası: {response.StatusCode}");
            }
        }

        // Müşteri için rezervasyon onay maili
        public async Task SendCustomerConfirmationEmailAsync(string customerEmail, DateTime appointmentDate, string serviceName, string appointmentId)
        {
            string subject = "🎉 Rezervasyon Onayı - Randevunuz Hazır!";

            string body = $@"
<div style='font-family: Arial, sans-serif; padding: 30px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 15px; max-width: 600px; margin: auto; color: white;'>
    <div style='text-align: center; background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; margin-bottom: 20px;'>
        <h1 style='margin: 0; font-size: 28px;'>🎉 Rezervasyon Onaylandı!</h1>
        <p style='margin: 10px 0 0 0; font-size: 16px; opacity: 0.9;'>Randevunuz başarıyla oluşturuldu</p>
    </div>

    <div style='background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; margin-bottom: 15px;'>
        <h3 style='margin: 0 0 15px 0; font-size: 20px;'>📋 Randevu Detayları</h3>
        <p style='margin: 8px 0; font-size: 16px;'><strong>📅 Tarih:</strong> {appointmentDate:dd MMMM yyyy}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>⏰ Saat:</strong> {appointmentDate:HH:mm}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>🎯 Hizmet:</strong> {serviceName}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>🔗 Randevu ID:</strong> {appointmentId}</p>
    </div>

    <div style='background: rgba(255,255,255,0.1); padding: 15px; border-radius: 10px; margin-bottom: 15px;'>
        <h4 style='margin: 0 0 10px 0;'>ℹ️ Önemli Hatırlatmalar</h4>
        <ul style='margin: 0; padding-left: 20px;'>
            <li>Randevunuzdan 15 dakika önce gelebilirsiniz</li>
            <li>İptal için en az 2 saat önceden bildiriniz</li>
            <li>Sorularınız için bizimle iletişime geçin</li>
        </ul>
    </div>

    <div style='text-align: center; padding: 20px; background: rgba(255,255,255,0.1); border-radius: 10px;'>
        <p style='margin: 0; font-size: 14px; opacity: 0.8;'>
            Bu e-posta otomatik olarak gönderilmiştir.<br>
            Herhangi bir sorunuz varsa bizimle iletişime geçin.
        </p>
    </div>
</div>";

            await SendEmailAsync(subject, body, customerEmail);
        }

        // Admin için yeni rezervasyon bildirim maili
        public async Task SendAdminNotificationEmailAsync(string adminEmail, string customerEmail, DateTime appointmentDate, string serviceName, string appointmentId)
        {
            string subject = "🔔 Yeni Rezervasyon Bildirimi";

            string body = $@"
<div style='font-family: Arial, sans-serif; padding: 30px; background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%); border-radius: 15px; max-width: 600px; margin: auto; color: white;'>
    <div style='text-align: center; background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; margin-bottom: 20px;'>
        <h1 style='margin: 0; font-size: 28px;'>🔔 Yeni Rezervasyon Geldi!</h1>
        <p style='margin: 10px 0 0 0; font-size: 16px; opacity: 0.9;'>Sisteme yeni bir randevu kaydedildi</p>
    </div>

    <div style='background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; margin-bottom: 15px;'>
        <h3 style='margin: 0 0 15px 0; font-size: 20px;'>👤 Müşteri Bilgileri</h3>
        <p style='margin: 8px 0; font-size: 16px;'><strong>📧 E-posta:</strong> {customerEmail}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>🔗 Randevu ID:</strong> {appointmentId}</p>
    </div>

    <div style='background: rgba(255,255,255,0.1); padding: 20px; border-radius: 10px; margin-bottom: 15px;'>
        <h3 style='margin: 0 0 15px 0; font-size: 20px;'>📋 Randevu Detayları</h3>
        <p style='margin: 8px 0; font-size: 16px;'><strong>📅 Tarih:</strong> {appointmentDate:dd MMMM yyyy}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>⏰ Saat:</strong> {appointmentDate:HH:mm}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>🎯 Hizmet:</strong> {serviceName}</p>
        <p style='margin: 8px 0; font-size: 16px;'><strong>⏱️ Oluşturulma:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
    </div>

    <div style='background: rgba(255,255,255,0.1); padding: 15px; border-radius: 10px; margin-bottom: 15px;'>
        <h4 style='margin: 0 0 10px 0;'>📝 İşlem Gereken</h4>
        <ul style='margin: 0; padding-left: 20px;'>
            <li>Randevu onayını kontrol edin</li>
            <li>Gerekirse müşteriyle iletişime geçin</li>
            <li>Randevu öncesi hazırlikları yapın</li>
        </ul>
    </div>

    <div style='text-align: center; padding: 20px; background: rgba(255,255,255,0.1); border-radius: 10px;'>
        <p style='margin: 0; font-size: 14px; opacity: 0.8;'>
            Bu bildirim otomatik olarak gönderilmiştir.<br>
            Admin panelinden randevu detaylarını görüntüleyebilirsiniz.
        </p>
    </div>
</div>";

            await SendEmailAsync(subject, body, adminEmail);
        }

        // Toplu mail gönderimi - hem müşteri hem admin
        public async Task SendReservationEmailsAsync(string customerEmail, string adminEmail, DateTime appointmentDate, string serviceName, string appointmentId)
        {
            // Paralel olarak her iki maili de gönder
            var customerEmailTask = SendCustomerConfirmationEmailAsync(customerEmail, appointmentDate, serviceName, appointmentId);
            var adminEmailTask = SendAdminNotificationEmailAsync(adminEmail, customerEmail, appointmentDate, serviceName, appointmentId);

            await Task.WhenAll(customerEmailTask, adminEmailTask);
        }
    }
}