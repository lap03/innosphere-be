namespace innosphere_be.Models.Requests.AuthRequest
{
    public class GoogleLoginRequest
    {
        public string IdToken { get; set; }
        public string Type { get; set; }
        public string? fullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}