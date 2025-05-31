
namespace Service.Models.EmployerModels
{
    public class EmployerEditModel
    {
        public string CompanyName { get; set; }
        public int? BusinessTypeId { get; set; } // FE truyền nếu chọn loại có sẵn
        public string? NewBusinessTypeName { get; set; }
        public string? NewBusinessTypeDescription { get; set; }

        public string? CompanyAddress { get; set; }
        public string? TaxCode { get; set; }
        public string? CompanyDescription { get; set; }
    }
}
