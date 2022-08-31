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
            if (string.IsNullOrWhiteSpace(addedUser.Email)
                || string.IsNullOrWhiteSpace(addedUser.Password)
                || string.IsNullOrWhiteSpace(addedUser.FirstName)
                || string.IsNullOrWhiteSpace(addedUser.LastName))
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
