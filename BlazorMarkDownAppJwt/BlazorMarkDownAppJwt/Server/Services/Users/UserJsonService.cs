using BlazorMarkDownAppJwt.Server.Entities;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
    public class UserJsonService : IUserService
    {
		private readonly IWebHostEnvironment env;

		private const string userPath = "Datas\\Json\\Users";
		public UserJsonService(IWebHostEnvironment env) => this.env = env;
		private static string CreateHash(string password)
		{
			var salt = "997eff51db1544c7a3c2ddeb2053f052";
			var md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt + password));
			byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(data);
		}
		public async Task<User?> AuthenticateUser(string? email, string? password)
		{
			try
			{
				if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
					return null;
				var path = Path.Combine(env.ContentRootPath, userPath);
				if (!Directory.Exists(path))
					return null;
				path = Path.Combine(path, email);
				if (!File.Exists(path))
					return null;
				User? existingUser = JsonConvert.DeserializeObject<User?>(await File.ReadAllTextAsync(path));
				if (existingUser != null && existingUser.Password == CreateHash(password))
				{
					return existingUser;
				}
				else
				{
					return null;
				}
			}
			catch
			{
				return null;
			}

		}
		public async Task<User?> AddUser(User addedUser)
		{
			try
			{
				if (string.IsNullOrEmpty(addedUser.Email) || string.IsNullOrEmpty(addedUser.Password))
					return null;
				var path = Path.Combine(env.ContentRootPath, userPath); // NOTE: THIS WILL CREATE THE "USERS" FOLDER IN THE PROJECT'S FOLDER!!!
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path); // NOTE: MAKE SURE THERE ARE CREATE/WRITE PERMISSIONS
				path = Path.Combine(path, addedUser.Email);
				if (File.Exists(path))
					return null;
				addedUser.Password = CreateHash(addedUser.Password);

				await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(addedUser));
				return addedUser;
			}
			catch
			{
				return null;
			}
		}
	}
}