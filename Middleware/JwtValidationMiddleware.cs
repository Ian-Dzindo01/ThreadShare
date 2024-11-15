using Azure;
using ThreadShare.Service.Implementations;
using ThreadShare.Service.Interfaces;

namespace ThreadShare.Middleware
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtValidationMiddleware(RequestDelegate next, ITokenService tokenService,
                                       IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var client = _httpClientFactory.CreateClient();
            var jwt = _tokenService.GetJwt();
            Console.WriteLine(jwt);
            if (jwt == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization token is missing.");
                return;
            }

            if (_tokenService.TokenIsExpired(jwt))
            {
                // Attempt to refresh the token
                jwt = await _tokenService.RefreshJwt();

                if (jwt == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unable to refresh token.");
                    return;
                }
            }
            _tokenService.SetJwt(jwt);
            await _next(context);
        }
    }
}
