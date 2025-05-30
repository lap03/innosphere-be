using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.CityModels
{
    public class CityModel
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
    }
}
