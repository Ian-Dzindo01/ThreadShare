using ThreadShare.Service.Interfaces;

namespace ThreadShare.Middleware
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;

        public JwtValidationMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            // Fix scope error
            var tokenService = serviceProvider.GetRequiredService<ITokenService>();
            var client = _httpClientFactory.CreateClient();

            try
            {
                var jwt = tokenService.GetJwt();

                if (jwt == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authorization token is missing.");
                    return;
                }

                if (tokenService.TokenIsExpired(jwt))
                {
                    jwt = await tokenService.RefreshJwt();

                    if (jwt == null)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Unable to refresh token.");
                        return;
                    }
                }

                tokenService.SetJwt(jwt);
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Middleware error: {ex.Message}");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An error occurred while processing your request.");
            }
        }
    }
}