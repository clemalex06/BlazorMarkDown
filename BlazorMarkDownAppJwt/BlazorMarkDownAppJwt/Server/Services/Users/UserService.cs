using BlazorMarkDownAppJwt.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
	public class UserSqlService : IUserService
	{
		private DataBaseContext Ctx { get; set; }

		public UserSqlService(DataBaseContext ctx)
        {
            Ctx = ctx;
        }

        public async Task<User?> AddUser(User addedUser)
		{
            try
            {
				if (string.IsNullOrEmpty(addedUser.Email)
					|| string.IsNullOrEmpty(addedUser.Password)
					|| string.IsNullOrEmpty(addedUser.FirstName)
					|| string.IsNullOrEmpty(addedUser.FirstName))
					return null;

				var existingUser = await Ctx.Users.SingleOrDefaultAsync(u => u.Email == addedUser.Email);
				if (existingUser != null) return null;

				Ctx.Users.Add(addedUser);
				await Ctx.SaveChangesAsync();
				return addedUser;
			}
            catch
            {
				return null;
            }
		}

		public async Task<User?> AuthenticateUser(string? email, string? password)
		{
            try
            {
				var user = await Ctx.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
				return user;
			}
            catch
            {
				return null;
            }
		}
	}
}
