using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models;
using Application.Queries.TimeSlot;
using Application.Queries.user;
using Application.RequestModels.Appointment;
using Application.RequestModels.Appointment.Create;
using Application.RequestModels.BlogPost.CreateBlog;
using Application.RequestModels.BlogPost.UpdateBlog;
using Application.RequestModels.BlogTag.Create;
using Application.RequestModels.Service.Create;
using Application.RequestModels.Service.Update;
using Application.RequestModels.TimeSlotCommand.Create;
using Application.RequestModels.TimeSlotCommand.Update;
using Application.RequestModels.User.CreateUser;
using Application.RequestModels.User.UpdateUser;
using AutoMapper;
using Domain.Models;
using HotelVR.Common.Infrastructure.Models.Queries;
using static Application.Enums.Role;

namespace Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>()
                .ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Availability, TimeSlotDto>()

               .ReverseMap();
            CreateMap<UpdateAvailableTimeSlotCommand, Availability>();
            #region service 
            CreateMap<Service, ServiceDto>()
              .ReverseMap();
            CreateMap<UpdateServiceCommand, Service>();
            CreateMap<CreateServiceCommand, Service>();
            #endregion
            CreateMap<CreateUserCommand, User>().ReverseMap();
            // Reservation -> ReservationDto Mapping
            CreateMap<CreateAppointmentCommand, Appointment>().ReverseMap();
            // Reservation -> ReservationDto Mapping

            CreateMap<AddAvailableTimeSlotCommand, Availability>().ReverseMap();

            CreateMap<Appointment, AppointmentDto>().ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId));
            CreateMap<User, UserDto>();

            CreateMap<CreateBlogPostCommand, BlogPost>()
            .ForMember(dest => dest.BlogPostTags, opt => opt.Ignore())
            .ForMember(dest => dest.Slug, opt => opt.Ignore()); ;// Slug'ı manuel setleyeceğiz

            CreateMap<CreateBlogTagCommand,BlogTag>()
                .ReverseMap();
            CreateMap<UpdateBlogPostComamnd, BlogPost>()
           .ForMember(dest => dest.Slug, opt => opt.Ignore()) // Slug'ı manuel setleyeceğiz
           .ForMember(dest => dest.UpdateDate, opt => opt.Ignore()); // Bunu da biz setleyeceğiz

            CreateMap<ForgotPasswordComand, User>();

            CreateMap<BlogTag, BlogTagDto>();
            CreateMap<GetALLTimeSlotQuery, Availability>();
            CreateMap<UpdateUserCommand, User>();

            CreateMap<BlogPost, BlogPostDto>();
        }
    }
}
