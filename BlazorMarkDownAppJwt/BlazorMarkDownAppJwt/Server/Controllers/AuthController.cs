using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorMarkDownAppJwt.Shared;
using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.Users;
using BlazorMarkDownAppJwt.Server.Helpers;

namespace BlazorMarkDownAppJwt.Server.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {


        private IUserService UserService { get; }

        public AuthController(IUserService userService)
        {
            this.UserService = userService;
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<LoginResult> PostAsync([FromBody] RegModel reg, CancellationToken cancellationToken)
        {
            try
            {
                if (reg.password != reg.confirmpwd)
                    return new LoginResult { message = "Password and confirm password do not match.", success = false };
                var regUser = new User
                {
                    Email = reg.email,
                    Password = reg.password,
                    FirstName = reg.firstName,
                    LastName = reg.lastName,

                };
                User? newuser = await UserService.AddUserAsync(regUser, cancellationToken);
                if (newuser != null)
                    return new LoginResult { message = "Registration successful.", jwtBearer = JWTHelper.CreateJWT(newuser), email = reg.email, success = true };
                return new LoginResult { message = "User already exists.", success = false };

            }
            catch (Exception ex)
            {
                return new LoginResult { message = ex.Message, success = false };
            }
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<LoginResult> PostAsync([FromBody] LoginModel log, CancellationToken cancellationToken)
        {
            try
            {
                User? user = await UserService.AuthenticateUserAsync(log.email, log.password, cancellationToken);
                if (user != null)
                    return new LoginResult { message = "Login successful.", jwtBearer = JWTHelper.CreateJWT(user), email = log.email, success = true };
                return new LoginResult { message = "User/password not found.", success = false };
            }
            catch (Exception ex)
            {
                return new LoginResult { message = ex.Message, success = false };
            }
        }
    }
}