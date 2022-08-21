// AuthModel.cs
using System.ComponentModel.DataAnnotations;

namespace BlazorMarkDownAppJwt.Shared
{
	public class LoginResult
	{
		public string message { get; set; }
		public string email { get; set; }
		public string jwtBearer { get; set; }
		public bool success { get; set; }
	}
	public class LoginModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Email address is not valid.")]
		public string email { get; set; } // NOTE: email will be the username, too

		[Required(ErrorMessage = "Password is required.")]
		[DataType(DataType.Password)]
		public string password { get; set; }
	}
	public class RegModel : LoginModel
	{
		[Required(ErrorMessage = "First Name is required.")]
		public string firstName { get; set; } // NOTE: for the moment not used

		[Required(ErrorMessage = "Last Name is required.")]
		public string lastName { get; set; } // NOTE: for the moment not used

		[Required(ErrorMessage = "Confirm password is required.")]
		[DataType(DataType.Password)]
		[Compare("password", ErrorMessage = "Password and confirm password do not match.")]
		public string confirmpwd { get; set; }
	}
}