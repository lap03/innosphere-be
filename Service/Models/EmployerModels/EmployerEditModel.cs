
using Service.Models.SocialLinkModels;

namespace Service.Models.EmployerModels
{
    public class EmployerEditModel
    {
        // User info
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        // Employer info
        public string CompanyName { get; set; }
        public int? BusinessTypeId { get; set; } // FE truyền nếu chọn loại có sẵn
        public string? NewBusinessTypeName { get; set; }
        public string? NewBusinessTypeDescription { get; set; }
        public string? CompanyAddress { get; set; }
        public string? TaxCode { get; set; }
        public string? CompanyDescription { get; set; }

        // Social Links (optional, FE có thể gửi List để tạo/update cùng profile)
        public List<CreateSocialLinkModel> SocialLinks { get; set; } = new List<CreateSocialLinkModel>();
    }
}
