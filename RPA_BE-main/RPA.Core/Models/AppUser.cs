using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Core.Models
{
    public class AppUser : IdentityUser 
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;

    }
}
