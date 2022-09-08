using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.Users
{
    public interface IUserService
    {
        Task<User?> AuthenticateUserAsync(string? email, string? password, CancellationToken cancellationToken);
        Task<User?> AddUserAsync(User user, CancellationToken cancellationToken);
    }
}
