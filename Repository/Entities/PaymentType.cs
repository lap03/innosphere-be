using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class PaymentType : BaseEntity
    {
        [Required(ErrorMessage = "Type name is required.")]
        [StringLength(100, ErrorMessage = "Type name cannot exceed 100 characters.")]
        public string TypeName { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
