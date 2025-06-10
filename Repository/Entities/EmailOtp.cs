using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class EmailOtp : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Otp { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; }
        public string? Purpose { get; set; } // e.g., "email_verification", "forgot_password", etc.
    }
}
