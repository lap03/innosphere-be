namespace Service.Models.UserModels
{
    public class UserWithRoleModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string IsDeleted { get; set; }
        public List<string> Roles { get; set; }
    }
}