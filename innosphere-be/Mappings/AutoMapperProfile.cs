using AutoMapper;
using innosphere_be.Models.responses.BusinessTypeResponses;
using Repository.Entities;
using Service.Models.AdvertisementPackageModels;
using Service.Models.CityModels;
using Service.Models.EmployerModels;
using Service.Models.JobPostings;
using Service.Models.JobTagModels;
using Service.Models.NotificationModels;
using Service.Models.PaymentModels;
using Service.Models.SocialLinkModels;
using Service.Models.SubscriptionPackageModels;
using Service.Models.WorkerModels;

namespace innosphere_be.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // worker profile
            CreateMap<Worker, WorkerEditModel>().ReverseMap();
            CreateMap<Worker, WorkerModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.WorkerId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<WorkerEditModel, Worker>()
                .ForMember(dest => dest.Skills, opt => opt.Condition(src => src.Skills != null))
                .ForMember(dest => dest.Bio, opt => opt.Condition(src => src.Bio != null))
                .ForMember(dest => dest.Education, opt => opt.Condition(src => src.Education != null))
                .ForMember(dest => dest.Experience, opt => opt.Condition(src => src.Experience != null))
                .ReverseMap();

            CreateMap<WorkerEditModel, User>()
                .ForMember(dest => dest.FullName, opt => opt.Condition(src => src.FullName != null))
                .ForMember(dest => dest.AvatarUrl, opt => opt.Condition(src => src.AvatarUrl != null))
                .ForMember(dest => dest.Address, opt => opt.Condition(src => src.Address != null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null))
                .ReverseMap();

            // employer profile
            CreateMap<Employer, EmployerEditModel>().ReverseMap();

            CreateMap<Employer, EmployerModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.EmployerId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<EmployerEditModel, User>()
                .ForMember(dest => dest.FullName, opt => opt.Condition(src => src.FullName != null))
                .ForMember(dest => dest.AvatarUrl, opt => opt.Condition(src => src.AvatarUrl != null))
                .ForMember(dest => dest.Address, opt => opt.Condition(src => src.Address != null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null))
                .ReverseMap();

            // BusinessType
            CreateMap<BusinessType, BusinessTypeResponse>().ReverseMap();

            // City
            CreateMap<City, CityModel>().ReverseMap();
            CreateMap<City, CreateCityModel>().ReverseMap();
            CreateMap<City, UpdateCityModel>().ReverseMap();

            // JobTag
            CreateMap<JobTag, JobTagModel>().ReverseMap();
            CreateMap<JobTag, CreateJobTagModel>().ReverseMap();
            CreateMap<JobTag, UpdateJobTagModel>().ReverseMap();

            // Payment
            CreateMap<Payment, PaymentModel>().ReverseMap();
            CreateMap<Payment, CreatePaymentModel>().ReverseMap();
            CreateMap<Payment, UpdatePaymentModel>().ReverseMap();

            // SubscriptionPackage
            CreateMap<SubscriptionPackage, SubscriptionPackageModel>().ReverseMap();
            CreateMap<SubscriptionPackage, CreateSubscriptionPackageModel>().ReverseMap();
            CreateMap<SubscriptionPackage, UpdateSubscriptionPackageModel>().ReverseMap();

            // AdvertisementPackage
            CreateMap<AdvertisementPackage, AdvertisementPackageModel>().ReverseMap();
            CreateMap<AdvertisementPackage, CreateAdvertisementPackageModel>().ReverseMap();
            CreateMap<AdvertisementPackage, UpdateAdvertisementPackageModel>().ReverseMap();

            // Notification
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Notification, CreateNotificationModel>().ReverseMap();
            CreateMap<Notification, UpdateNotificationModel>().ReverseMap();

            // SocialLink
            CreateMap<SocialLink, SocialLinkModel>().ReverseMap();
            CreateMap<SocialLink, CreateSocialLinkModel>().ReverseMap();
            CreateMap<SocialLink, UpdateSocialLinkModel>().ReverseMap();

            // JobPosting
            CreateMap<CreateJobPostingModel, JobPosting>().ReverseMap();
            CreateMap<JobPosting, JobPostingModel>()
                .ForMember(dest => dest.JobTags, opt => opt.MapFrom(src => src.JobPostingTags.Select(jpt => jpt.JobTag)))
                .ReverseMap();
        }
    }
}
