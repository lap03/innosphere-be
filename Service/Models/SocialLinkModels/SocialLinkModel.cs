using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SocialLinkModels
{
    public class SocialLinkModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }
    }
}
