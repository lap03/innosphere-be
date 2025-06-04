using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters.")]
        public string FullName { get; set; }

        [StringLength(512, ErrorMessage = "Avatar URL cannot exceed 512 characters.")]
        public string? AvatarUrl { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string? Address { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Worker Worker { get; set; }
        public virtual Employer Employer { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<SocialLink> SocialLinks { get; set; }
    }
}
