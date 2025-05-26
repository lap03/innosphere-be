using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.AuthModels
{
    public class JwtModel
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserID { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
