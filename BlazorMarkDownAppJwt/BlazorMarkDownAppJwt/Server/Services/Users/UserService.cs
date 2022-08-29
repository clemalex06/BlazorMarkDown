using BlazorMarkDownAppJwt.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
    public class UserService : IUserService
    {
        private DataBaseContext Ctx { get; set; }

        public UserService(DataBaseContext ctx)
        {
            Ctx = ctx;
        }

        public async Task<User?> AddUser(User addedUser)
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

        public async Task<User?> AuthenticateUser(string? email, string? password)
        {
            var user = await Ctx.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }
    }
}
