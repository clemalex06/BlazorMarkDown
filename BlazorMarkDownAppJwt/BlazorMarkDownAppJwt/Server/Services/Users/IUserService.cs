using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
    public interface IUserService
    {
        Task<User?> AuthenticateUser(string? email, string? password);
        Task<User?> AddUser(User user);
    }
}
