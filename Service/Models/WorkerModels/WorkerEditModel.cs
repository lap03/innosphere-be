using Service.Models.SocialLinkModels;

namespace Service.Models.WorkerModels
{
    public class WorkerEditModel
    {
        // User info
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        // Worker info
        public string? Skills { get; set; }
        public string? Bio { get; set; }
        public string? Education { get; set; }
        public string? Experience { get; set; }

        // Social Links (optional, FE có thể gửi List để tạo/update cùng profile)
        public List<CreateSocialLinkModel> SocialLinks { get; set; } = new List<CreateSocialLinkModel>();
    }
}

