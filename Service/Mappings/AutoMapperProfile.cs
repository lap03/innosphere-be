using AutoMapper;
using Repository.Entities;
using Service.Models.AdvertisementPackageModels;
using Service.Models.CityModels;
using Service.Models.JobTagModels;
using Service.Models.NotificationModels;
using Service.Models.PaymentModels;
using Service.Models.PaymentTypeModels;
using Service.Models.SubscriptionPackageModels;
using Service.Models.SocialLinkModels;

namespace innosphere_be.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
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

            // PaymentType
            CreateMap<PaymentType, PaymentTypeModel>().ReverseMap();
            CreateMap<PaymentType, CreatePaymentTypeModel>().ReverseMap();
            CreateMap<PaymentType, UpdatePaymentTypeModel>().ReverseMap();

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
        }
    }
}
