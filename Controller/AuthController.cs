using Microsoft.AspNetCore.Mvc;
using ThreadShare.Service.Interfaces;
using ThreadShare.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ThreadShare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(ITokenService tokenService, IUserService userService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = await _userService.GetUserById(userIdClaim);
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {

                return Unauthorized("Invalid Refresh Token");
            }

            else if (user.TokenExpires < DateTime.UtcNow)
            {
                return Unauthorized("Refresh Token Expired");
            }

            string token = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Set refresh token
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            return Ok(token);
        }
    }
}
   