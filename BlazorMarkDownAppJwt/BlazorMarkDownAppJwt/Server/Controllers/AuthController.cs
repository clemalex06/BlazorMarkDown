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
            UserService = userService;
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<ActionResult<LoginResult>> PostAsync([FromBody] RegModel reg, CancellationToken cancellationToken)
        {
            try
            {
                if (reg.Password != reg.Confirmpwd)
                    return Ok(new LoginResult { Message = "Password and confirm password do not match.", Success = false });
                var regUser = new User
                {
                    Email = reg.Email,
                    Password = reg.Password,
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,

                };
                User? newuser = await UserService.AddUserAsync(regUser, cancellationToken);
                if (newuser != null)
                    return Ok(new LoginResult { Message = "Registration successful.", JwtBearer = JWTHelper.CreateJWT(newuser), Email = reg.Email, Success = true });
                return Ok(new LoginResult { Message = "User already exists.", Success = false });

            }
            catch (Exception ex)
            {
                return Ok(new LoginResult { Message = ex.Message, Success = false });
            }
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<ActionResult<LoginResult>> PostAsync([FromBody] LoginModel log, CancellationToken cancellationToken)
        {
            try
            {
                User? user = await UserService.AuthenticateUserAsync(log.Email, log.Password, cancellationToken);
                if (user != null)
                    return Ok(new LoginResult { Message = "Login successful.", JwtBearer = JWTHelper.CreateJWT(user), Email = log.Email, Success = true });
                return Ok(new LoginResult { Message = "User/password not found.", Success = false });
            }
            catch (Exception ex)
            {
                return Ok(new LoginResult { Message = ex.Message, Success = false });
            }
        }
    }
}