using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Page;
using Application.RequestModels.Appointment;
using Application.RequestModels.Appointment.Create;
using Application.RequestModels.Appointment.UpdateAppoıntmentStatus;
using Application.RequestModels.Service.Create;
using Application.RequestModels.TimeSlotCommand.Create;
using HotelVR.Common.Infrastructure.Exceptions;
using HotelVR.WebApp.Infrastructure.Services.Interfaceses;
using static System.Net.WebRequestMethods;

namespace HotelVR.WebApp.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AppointmentService> _logger;



        public AppointmentService(IHttpClientFactory httpClientFactory, ILogger<AppointmentService> logger = null)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _logger = logger;
        }

        public async Task<Guid> CreateAppointmentAsync(CreateAppointmentCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/appointments/CreateAppointment", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<List<TimeSlotDto>> GetAvailableTimesAsync(DateTime SelectedDate)
        {
            return await _httpClient.GetFromJsonAsync<List<TimeSlotDto>>($"api/Appointments/get-available-slots?date={SelectedDate:yyyy-MM-ddTHH:mm:ss}");
        }

        public async Task<PagedViewModel<AppointmentDto>> getPaginatedAppointment(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PagedViewModel<AppointmentDto>>($"api/Appointments/User-Appointment?page={page}&pageSize={pageSize}");
            return response;

        }

        // AppointmentService.cs
        public async Task<AppointmentDto> GetByIdAppointmentsDetail(Guid Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/appointments/{Id}");
                response.EnsureSuccessStatusCode();

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new AppointmentDto(); // veya null dönebilirsiniz
                }

                return await response.Content.ReadFromJsonAsync<AppointmentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting appointment: {ex.Message}");
                return new AppointmentDto(); // veya null
            }
        }
        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AppointmentDto>>("api/appointments/get-all-appointments");
        }
        public async Task<List<ServiceDto>> GetServicesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ServiceDto>>($"api/appointments/get-services");
        }
        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/appointments/delete-appointments/{id}");
            response.EnsureSuccessStatusCode();
        }



        public async Task<Guid> CreateService(CreateServiceCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/appointments/create-service", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }
        public async Task<List<ServiceDto>> GetAllServices()
        {
            var response = await _httpClient.GetAsync("api/appointments/get-all-services");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ServiceDto>>();
        }
        public async Task<ServiceDto> GetServiceById(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/appointments/get-service-by-id/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceDto>();
        }
        public async Task UpdateService(Guid id, CreateServiceCommand command)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/appointments/update-service/{id}", command);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteService(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/appointments/delete-service/{id}");
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteTimeSlot(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/appointments/delete-TimeSlot/{id}");
            response.EnsureSuccessStatusCode();

        }
        public async Task<List<AppointmentDto>> GetUserAppointments()
        {
            var response = await _httpClient.GetAsync($"api/appointments/get-Appointments-by-Userid");
            response.EnsureSuccessStatusCode();
            return await response.Content?.ReadFromJsonAsync<List<AppointmentDto>>();

        }
        // Mevcut servise ekle


        public async Task<bool> AddTimeSlotAsync(DateTime slot)
        {
            var response = await _httpClient.PostAsJsonAsync("api/appointments/add-available-slot",
                new { AppointmentDateTime = slot, IsBooked = true });
            response.EnsureSuccessStatusCode();

            // Return true if success, but you may want to add additional logic to check
            // if the slot was actually added or not
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDateAsync(DateTime? date)
        {
            var response = await _httpClient.GetAsync($"api/appointments/get-by-date/{date:yyyy-MM-ddTHH:mm:ss}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>();
        }


        public async Task<List<TimeSlotDto>> GetAllTimeSlot()
        {
            return await _httpClient.GetFromJsonAsync<List<TimeSlotDto>>("api/appointments/get-Time-slot");
        }
        public async Task<Guid> ConfirmAppointmentAsync(Guid Id)
        {
            try
            {
                var command = new ConfirmAppoıntmentStatusCommand // Typo düzeltildi
                {
                    Id = Id
                };

                // Debug için browser console'a log
                _logger.LogInformation($"ConfirmAppointment çağrılıyor. ID: {Id}");
                _logger.LogInformation($"Base Address: {_httpClient.BaseAddress}");

                var url = "api/appointments/confirm";
                _logger.LogInformation($"Full URL: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.PostAsJsonAsync(url, command);

                _logger.LogInformation($"Response Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API Error - Status: {response.StatusCode}, Content: {errorContent}");
                    throw new HttpRequestException($"API call failed: {response.StatusCode} - {errorContent}");
                }

                var result = await response.Content.ReadFromJsonAsync<Guid>();
                _logger.LogInformation($"ConfirmAppointment başarılı. Result: {result}");
                return result;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP Error in ConfirmAppointmentAsync: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Error in ConfirmAppointmentAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Guid> CancelAppointmentAsync(Guid Id)
        {
            try
            {
                var command = new CancelAppointmentStatusCommand
                {
                    Id = Id
                };

                _logger.LogInformation($"CancelAppointment çağrılıyor. ID: {Id}");
                _logger.LogInformation($"Base Address: {_httpClient.BaseAddress}");

                var url = "api/appointments/cancel";
                _logger.LogInformation($"Full URL: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.PostAsJsonAsync(url, command);

                _logger.LogInformation($"Response Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API Error - Status: {response.StatusCode}, Content: {errorContent}");
                    throw new HttpRequestException($"API call failed: {response.StatusCode} - {errorContent}");
                }

                var result = await response.Content.ReadFromJsonAsync<Guid>();
                _logger.LogInformation($"CancelAppointment başarılı. Result: {result}");
                return result;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP Error in CancelAppointmentAsync: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Error in CancelAppointmentAsync: {ex.Message}");
                throw;
            }
        }
    }

}

