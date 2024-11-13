using System.Text.Json;
using ThreadShare.Service.Interfaces;
using ThreadShare.DTOs.Data_Transfer;

namespace ThreadShare.Handlers
{
    public class JwtHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtHandler(ITokenService tokenService, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var jwt = _tokenService.GetJwt();

            if (_tokenService.TokenIsExpired(jwt))
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

                // Reset refreshed JWT
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthToken", jwt, new CookieOptions
                {
                    HttpOnly = true, // inaccessible to JavaScript for security
                    Secure = true,   // cookie is sent only over HTTPS (set to false for local testing if needed)
                    SameSite = SameSiteMode.Lax, // cookie sent with cross-site requests
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
