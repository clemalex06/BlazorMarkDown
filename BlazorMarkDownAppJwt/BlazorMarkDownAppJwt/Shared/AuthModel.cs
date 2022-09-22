using System.ComponentModel.DataAnnotations;

namespace BlazorMarkDownAppJwt.Shared
{
    public class LoginResult
    {
        public string? Message { get; set; }
        public string? Email { get; set; }
        public string? JwtBearer { get; set; }
        public bool Success { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
    public class RegModel : LoginModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string? Confirmpwd { get; set; }
    }
}