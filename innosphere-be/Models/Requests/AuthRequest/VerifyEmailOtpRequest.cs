

namespace innosphere_be.Models.Requests.AuthRequest
{
    public class VerifyEmailOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

}