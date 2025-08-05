using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Queries.Email;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("hucks.parament.2001@gmail.com", "mvws zbho sffk xatp"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("hucks.parament.2001@gmail.com"),
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(request.ToEmail);

                await smtpClient.SendMailAsync(mailMessage);

                return Ok(new { message = "E-posta başarıyla gönderildi!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
