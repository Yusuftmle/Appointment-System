
using Application.Models;
using Application.Queries.Appointments;
using Application.Queries.Blog;
using Application.Queries.Service;
using Application.Queries.TimeSlot;
using Application.RequestModels.Appointment;
using Application.RequestModels.Appointment.Create;
using Application.RequestModels.Appointment.Delete;
using Application.RequestModels.Appointment.UpdateAppoıntmentStatus;
using Application.RequestModels.BlogPost.DeleteBlog;
using Application.RequestModels.Service.Create;
using Application.RequestModels.Service.Delete;
using Application.RequestModels.Service.Update;
using Application.RequestModels.TimeSlotCommand.Create;
using Application.RequestModels.TimeSlotCommand.Delete;
using Application.RequestModels.TimeSlotCommand.Update;
using Domain.Models;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : BaseController
    {
        private readonly IMediator _mediator;


        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [HttpPost]
        [Route("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var guid = await _mediator.Send(command);
            return Ok(guid);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-Appointments/{id}")]
        public async Task<IActionResult> DeleteAppointments(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAppointmentCommand { Id = id });
                return Ok(result);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByAppointments(Guid Id)
        {
            var result = await _mediator.Send(new GetByIdAppointmentsQuery(Id));
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("Update-available-slot")]
        public async Task<IActionResult> UpdateAvailableTimeSlot([FromBody] UpdateAvailableTimeSlotCommand command)
        {
            try
            {
                var updated = await _mediator.Send(command);
                return Ok(updated);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("add-available-slot")]
        public async Task<IActionResult> AddAvailableTimeSlot([FromBody] AddAvailableTimeSlotCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest("Bu saat zaten mevcut.");
            }
            return Ok("Müsait saat başarıyla eklendi.");
        }
        [Authorize]
        [HttpGet("get-available-slots")]
        public async Task<IActionResult> GetAvailableTimeSlots([FromQuery] DateTime date)
        {
            var query = new TimeSlotQuery(date);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-all-appointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var query = new GetAllAppointmentsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Create-service")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command)
        {
            var guid = await _mediator.Send(command);
            return Ok(guid);
        }

        [HttpGet]
        [Authorize]
        [Route("User-Appointment")]
        public async Task<IActionResult> GetMainPageAppointments(int page, int pageSize)
        {
            var entries = await _mediator.Send(new GetPaginatedAppointmentsQuery(UserId, page, pageSize));

            return Ok(entries);

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
       [Route ("update-services")]
        public async Task<IActionResult> UpdateService([FromBody] UpdateServiceCommand command)
        {
            try
            {
                
                var updated = await _mediator.Send(command);
                return Ok(updated);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-services")]
        public async Task<IActionResult> GetServices()
        {
            var query = new GetServiceQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-service/{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteServiceCommand { Id = id });
                return Ok(result);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-TimeSlot/{id}")]
        public async Task<IActionResult> DeleteTimeSlot(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAvailableTimeSlotCommand { Id = id });
                return Ok(result);
            }
            catch (DataBaseValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpGet("get-Appointments-by-Userid")]
        public async Task<IActionResult> GetUserAppointments()
        {
            try
            {
                if (UserId == null)
                    return Unauthorized(new { message = "Geçersiz token!" });

                var query = new GetUserAppointmentsQuery(UserId.Value);

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch(DataBaseValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
           
        }
        [Authorize]
        [HttpGet("get-Time-slot")]
        public async Task<ActionResult<List<TimeSlotDto>>> getTimeslot()
        {
            var query = new GetALLTimeSlotQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmAppointment([FromBody] ConfirmAppoıntmentStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


    }
}


