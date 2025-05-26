using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace innosphere_be.Models.Requests.InitRequest
{
    public class AssignRoleRequest
    {
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
