using System.ComponentModel.DataAnnotations;

namespace BAL.ViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; } 
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
