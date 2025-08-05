using Application.Models;
using Application.Models.Page;
using Application.RequestModels.Appointment;
using Application.RequestModels.Appointment.Create;
using Application.RequestModels.BlogPost.UpdateBlog;
using Application.RequestModels.Service.Create;
using Application.RequestModels.TimeSlotCommand.Create;

namespace HotelVR.WebApp.Infrastructure.Services.Interfaceses
{
    public interface IAppointmentService
    {
        Task<Guid> CancelAppointmentAsync(Guid Id);
        Task DeleteService(Guid id);
        Task DeleteTimeSlot(Guid id);
        Task<Guid> ConfirmAppointmentAsync(Guid Id);
        Task<Guid> CreateAppointmentAsync(CreateAppointmentCommand command);
        Task<Guid> CreateService(CreateServiceCommand command);
        Task<List<TimeSlotDto>> GetAvailableTimesAsync(DateTime SelectedDate);
        Task<PagedViewModel<AppointmentDto>> getPaginatedAppointment(int page = 1, int pageSize = 10);
        Task<List<ServiceDto>> GetServicesAsync();
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();
        Task<AppointmentDto> GetByIdAppointmentsDetail(Guid Id);
        Task DeleteAsync(Guid id);
        Task<List<TimeSlotDto>> GetAllTimeSlot();
        Task<bool> AddTimeSlotAsync(DateTime slot);
        Task<List<AppointmentDto>> GetUserAppointments();
        Task<List<AppointmentDto>> GetAppointmentsByDateAsync(DateTime? date);
    }
}