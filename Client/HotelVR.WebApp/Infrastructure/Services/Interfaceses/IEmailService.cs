namespace HotelVR.WebApp.Infrastructure.Services.Interfaceses
{
    public interface IEmailService
    {
        // Temel mail gönderme metodu
        Task SendEmailAsync(string subject, string body, string toEmail);

        // Müşteri onay maili
        Task SendCustomerConfirmationEmailAsync(string customerEmail, DateTime appointmentDate, string serviceName, string appointmentId);

        // Admin bildirim maili
        Task SendAdminNotificationEmailAsync(string adminEmail, string customerEmail, DateTime appointmentDate, string serviceName, string appointmentId);

        // Hem müşteri hem admin maili - toplu gönderim
        Task SendReservationEmailsAsync(string customerEmail, string adminEmail, DateTime appointmentDate, string serviceName, string appointmentId);
    }
}