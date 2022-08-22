using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
	public class UserSqlService : IUserService
	{
		public Task<User?> AddUser(User user)
		{
			throw new NotImplementedException();
		}

		public Task<User?> AuthenticateUser(string? email, string? password)
		{
			throw new NotImplementedException();
		}
	}
}
