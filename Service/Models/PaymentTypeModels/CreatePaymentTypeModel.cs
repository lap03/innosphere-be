using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PaymentTypeModels
{
    public class CreatePaymentTypeModel
    {
        [Required(ErrorMessage = "Type name is required.")]
        [StringLength(100, ErrorMessage = "Type name cannot exceed 100 characters.")]
        public string TypeName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }
    }
}
