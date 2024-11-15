using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadShare.Models;
using ThreadShare.Service.Interfaces;
using System.Security.Cryptography;
using ThreadShare.DTOs.Data_Transfer;
using System.Text.Json;
using Azure;
using Microsoft.AspNetCore.Identity;


namespace ThreadShare.Service.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<TokenService> _logger;


        public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor,
                            IHttpClientFactory httpClientFactory, UserManager<User> userManager, ILogger<TokenService> logger)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _logger = logger;
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

        public async Task<string> RefreshJwt()
        {
            var client = _httpClientFactory.CreateClient();
            var jwt = GetJwt();

            if (TokenIsExpired(jwt))
            {
                var response = await client.PostAsync("api/auth/refresh-token", null);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var refreshedJwt = JsonSerializer.Deserialize<TokenResponse>(json);

                    if (!string.IsNullOrEmpty(refreshedJwt?.Token))
                    {
                        jwt = refreshedJwt.Token;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("Unable to refresh token.");
                    }
                }
            }
            return jwt;
        }

        public void SetJwt(string jwt)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthToken", jwt, new CookieOptions
            {
                HttpOnly = true,       // Cookie inaccessible to JavaScript for security
                Secure = true,         // Only sent over HTTPS; set to false for local testing if needed
                SameSite = SameSiteMode.Lax,  // Cookie sent with cross-site requests
                Expires = DateTimeOffset.UtcNow.AddHours(1)  // Set expiration time for the JWT
            });
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

        public async Task SetRefreshToken(RefreshToken newRefreshToken, string email)
        {

            var user = await _userManager.FindByEmailAsync(email);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to update refresh token in the database");
            }

            _logger.LogInformation($"Refresh token updated for user {user.UserName}");
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
