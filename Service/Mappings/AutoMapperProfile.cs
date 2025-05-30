using AutoMapper;
using Repository.Entities;
using Service.Models.AdvertisementPackageModels;
using Service.Models.CityModels;
using Service.Models.JobTagModels;
using Service.Models.PaymentModels;
using Service.Models.SubscriptionPackageModels;

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
            CreateMap<JobTag, JobTagModel>().ReverseMap().ReverseMap();
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
        }
    }
}
