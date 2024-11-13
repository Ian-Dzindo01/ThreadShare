using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadShare.Models;
using ThreadShare.Service.Interfaces;
using System.Security.Cryptography;

namespace ThreadShare.Service.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetJwtFromCookie()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["authToken"];
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetJwt()
        {
            return _httpContextAccessor.HttpContext.Request?.Cookies["AuthToken"];
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        public bool TokenIsExpired(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
            {
                return true;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(jwt);

                var exp = jsonToken.Payload.Exp;

                if (exp == null || exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
