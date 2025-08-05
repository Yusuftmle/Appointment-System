
using Application.Models;
using Application.Queries.ResetPassword;
using Application.Queries.user;
using Application.RequestModels.User.CreateUser;
using Application.RequestModels.User.LoginCommand;
using Application.RequestModels.User.PasswordComment;
using Application.RequestModels.User.UpdateUser;
using Domain.Models;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await mediator.Send(new AllUserListQuery());
            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
           
                command.UserId = UserId;
            if (!UserId.HasValue)
                return Unauthorized(new { error = "Kullanıcı kimliği bulunamadı, giriş yapmalısınız." });

            try
            {
                var guid = await mediator.Send(command);
                return Ok(guid);
            }
            catch (DataBaseValidationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

          
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var guid = await mediator.Send(command);
            return Ok(guid);
        }

        [Authorize]
        [HttpPost]
        [Route("User-Update")]

        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {

            command.Id = UserId.Value;
            var guid = await mediator.Send(command);
            return Ok(guid);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordComand command)
        {

            var result = await mediator.Send(command);

            if (result)
            {
                return Ok(new { message = "Şifre sıfırlama e-postası gönderildi" });
            }

            return BadRequest(new { message = "Şifre sıfırlama işlemi başarısız" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("UserDetail")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                if (UserId == null)
                    return Unauthorized(new { message = "Geçersiz token!" });

                var result = await mediator.Send(new UserDetaılQuery(UserId.Value));
                return Ok(result);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetCommand command)
        {
            var result = await mediator.Send(command);
            if (result.Succeeded)
                return Ok(result);

            return BadRequest(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await mediator.Send(command);
            if (result.Succeeded)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
