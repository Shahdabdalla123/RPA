using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTOs
{
    public class RegisterDTO
    {

        [Required(ErrorMessage ="FName is required")]
        public string FName { get; set; }

        [Required(ErrorMessage = "LName is required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string role { get; set; } = "Employee"; 


    }
}
