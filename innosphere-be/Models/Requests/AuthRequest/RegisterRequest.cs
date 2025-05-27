using System.ComponentModel.DataAnnotations;

namespace innosphere_be.Models.Requests.AuthRequest
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Address { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
