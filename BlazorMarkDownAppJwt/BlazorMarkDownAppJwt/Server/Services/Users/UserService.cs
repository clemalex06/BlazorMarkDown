using BlazorMarkDownAppJwt.Server.Entities;
using System.Security.Cryptography;
using System.Text;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
    public class UserService : IUserService
    {
		private readonly IWebHostEnvironment env;

		private const string userPath = "Datas\\Json\\Users";
		public UserService(IWebHostEnvironment env) => this.env = env;
		private static string CreateHash(string password)
		{
			var salt = "997eff51db1544c7a3c2ddeb2053f052";
			var md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt + password));
			byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
			return System.Convert.ToBase64String(data);
		}
		public async Task<User?> AuthenticateUser(string? email, string? password)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
				return null;
			var path = System.IO.Path.Combine(env.ContentRootPath, userPath);
			if (!System.IO.Directory.Exists(path))
				return null;
			path = System.IO.Path.Combine(path, email);
			if (!System.IO.File.Exists(path))
				return null;
			if (await System.IO.File.ReadAllTextAsync(path) != CreateHash(password))
				return null;
			return new User
			{
				Email = email,
			};
		}
		public async Task<User?> AddUser(User addedUser)
		{
			try
			{
				if (string.IsNullOrEmpty(addedUser.Email) || string.IsNullOrEmpty(addedUser.Password))
					return null;
				var path = System.IO.Path.Combine(env.ContentRootPath, userPath); // NOTE: THIS WILL CREATE THE "USERS" FOLDER IN THE PROJECT'S FOLDER!!!
				if (!System.IO.Directory.Exists(path))
					System.IO.Directory.CreateDirectory(path); // NOTE: MAKE SURE THERE ARE CREATE/WRITE PERMISSIONS
				path = System.IO.Path.Combine(path, addedUser.Email);
				if (System.IO.File.Exists(path))
					return null;
				await System.IO.File.WriteAllTextAsync(path, CreateHash(addedUser.Password));
				return addedUser;
			}
			catch
			{
				return null;
			}
		}
	}
}