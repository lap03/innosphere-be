
namespace innosphere_be.Models.Requests.AuthRequest
{
    public class ResetPasswordByOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}