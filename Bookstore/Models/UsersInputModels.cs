using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class SignInFields
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }

    public class SignUpFields
    {
        [Required(ErrorMessage = "Please choose a username.")]
        [MaxLength(15, ErrorMessage = "Username can not exceed 15 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please choose a password.")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@$!%*#?&/])[A-Za-z\\d$@$!%*#?&/]{8,}$", ErrorMessage = "Password must have 8+ characters, atleast 1 Alphabet 1 Number and 1 Special Character")]2
        [MaxLength(16)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-enter your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [MaxLength(16)]
        public string Password2 { get; set; }
    }
}
