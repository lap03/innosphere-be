using AutoMapper;
using innosphere_be.Models.responses.BusinessTypeResponses;
using Repository.Entities;
using Service.Models.AdvertisementModels;
using Service.Models.AdvertisementPackageModels;
using Service.Models.CityModels;
using Service.Models.EmployerModels;
using Service.Models.JobApplicationModels;
using Service.Models.JobPostings;
using Service.Models.JobTagModels;
using Service.Models.NotificationModels;
using Service.Models.SocialLinkModels;
using Service.Models.SubscriptionModels;
using Service.Models.SubscriptionPackageModels;
using Service.Models.WorkerModels;
using Service.Models.ResumeModels;
using Service.Models.RatingCriteriaModels;
using Service.Models.WorkerRatingModels;
using Service.Models.WorkerRatingCriteriaModels;
using Service.Models.EmployerRatingModels;
using Service.Models.EmployerRatingCriteriaModels;
using Service.Models.BusinessTypeModels;
using Service.Models.UserModels;

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
                .ForMember(dest => dest.SocialLinks, opt => opt.MapFrom(src => src.User.SocialLinks))
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
                .ForMember(dest => dest.SocialLinks, opt => opt.MapFrom(src => src.User.SocialLinks))

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

            // Advertisement
            CreateMap<Advertisement, AdvertisementModel>().ReverseMap();
            CreateMap<Advertisement, CreateAdvertisementModel>().ReverseMap();
            CreateMap<Advertisement, UpdateAdvertisementModel>().ReverseMap();

            //Subscription
            CreateMap<Subscription, SubscriptionModel>().ReverseMap();
            CreateMap<Subscription, CreateSubscriptionModel>().ReverseMap();
            CreateMap<Subscription, UpdateSubscriptionModel>().ReverseMap();

            // JobPosting
            CreateMap<CreateJobPostingModel, JobPosting>().ReverseMap();
            CreateMap<JobPosting, JobPostingModel>()
                .ForMember(dest => dest.JobTags, opt => opt.MapFrom(src => src.JobPostingTags.Select(jpt => jpt.JobTag)))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Employer.CompanyName))
                .ForMember(dest => dest.CompanyLogoUrl, opt => opt.MapFrom(src => src.Employer.CompanyLogoUrl))
                .ForMember(dest => dest.BusinessTypeId, opt => opt.MapFrom(src => src.Employer.BusinessTypeId))
                .ForMember(dest => dest.BusinessTypeName, opt => opt.MapFrom(src => src.Employer.BusinessType.Name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.CityName))
                .ReverseMap();

            //JobApplication
            // JobApplication
            CreateMap<JobApplication, JobApplicationModel>()
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src =>
                    src.JobPosting != null ? src.JobPosting.Title : string.Empty))
                .ForMember(dest => dest.WorkerName, opt => opt.MapFrom(src =>
                    src.Worker != null && src.Worker.User != null ? src.Worker.User.FullName : string.Empty))
                .ForMember(dest => dest.ResumeTitle, opt => opt.MapFrom(src =>
                    src.Resume != null ? src.Resume.Title : string.Empty))
                // ✅ Lồng jobPosting vào model
                .ForMember(dest => dest.JobPosting, opt => opt.MapFrom(src => src.JobPosting))
                .ReverseMap();

            CreateMap<CreateJobApplicationModel, JobApplication>()
                .ForMember(dest => dest.AppliedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Worker, opt => opt.Ignore())
                .ForMember(dest => dest.Employer, opt => opt.Ignore())
                .ForMember(dest => dest.JobPosting, opt => opt.Ignore())
                .ForMember(dest => dest.Resume, opt => opt.Ignore());

            CreateMap<UpdateJobApplicationStatusModel, JobApplication>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
                
            // Resume
            CreateMap<Resume, ResumeModel>().ReverseMap();
            CreateMap<CreateResumeModel, Resume>().ReverseMap();

            // RatingCriteria
            CreateMap<RatingCriteria, RatingCriteriaModel>().ReverseMap();
            CreateMap<CreateRatingCriteriaModel, RatingCriteria>().ReverseMap();

            // WorkerRating <-> WorkerRatingModel
            CreateMap<WorkerRating, WorkerRatingModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ReverseMap();

            // WorkerRatingCriteria <-> WorkerRatingCriteriaModel
            CreateMap<WorkerRatingCriteria, WorkerRatingCriteriaModel>()
                .ForMember(dest => dest.CriteriaName, opt => opt.MapFrom(src => src.RatingCriteria.CriteriaName))
                .ForMember(dest => dest.CriteriaDescription, opt => opt.MapFrom(src => src.RatingCriteria.Description))
                .ReverseMap();

            // EmployerRating <-> EmployerRatingModel
            CreateMap<EmployerRating, EmployerRatingModel>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ReverseMap();

            // EmployerRatingCriteria <-> EmployerRatingCriteriaModel
            CreateMap<EmployerRatingCriteria, EmployerRatingCriteriaModel>()
                .ForMember(dest => dest.CriteriaName, opt => opt.MapFrom(src => src.RatingCriteria.CriteriaName))
                .ForMember(dest => dest.CriteriaDescription, opt => opt.MapFrom(src => src.RatingCriteria.Description))
                .ReverseMap();

            // BusinessType
            CreateMap<BusinessType, BusinessTypeModel>().ReverseMap();
            CreateMap<BusinessType, CreateBusinessTypeModel>().ReverseMap();
            CreateMap<BusinessType, UpdateBusinessTypeModel>().ReverseMap();

            // User
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, UserWithRoleModel>().ReverseMap();
            CreateMap<User, UpdateUserModel>()
                .ForMember(dest => dest.FullName, opt => opt.Condition(src => src.FullName != null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.PhoneNumber != null))
                .ReverseMap();
        }
    }
}
