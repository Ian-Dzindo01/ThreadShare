public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next,
                                            ILogger<GlobalExceptionHandlerMiddleware> logger,
                                            IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;  
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = 500;

            var responseMessage = _env.IsDevelopment()
                ? ex.ToString() 
                : "An unexpected error occurred."; 

            await context.Response.WriteAsync(responseMessage);
        }
    }
}
