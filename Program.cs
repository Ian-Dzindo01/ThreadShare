using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThreadShare.Data;
using ThreadShare.Handlers;
using ThreadShare.Models;
using ThreadShare.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
// FIx postgre DateTime problem
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddSwaggerGen();
Host.CreateDefaultBuilder(args).UseDefaultServiceProvider(options => options.ValidateScopes = true);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql
                (configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});


builder.Services.AddHttpClient();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtHandler>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IPostRepository>() 
    .AddClasses(classes => classes.InNamespaces("ThreadShare.Repository", "ThreadShare.Service")) 
    .AsImplementedInterfaces() 
    .WithScopedLifetime()
);


builder.Services.AddHttpClient("ApiHttpClient")
        .AddHttpMessageHandler<JwtHandler>();

builder.Services.AddControllersWithViews();

string jwtSigningKey = configuration["JWT:SigningKey"];
if (string.IsNullOrEmpty(jwtSigningKey))
{
    throw new InvalidOperationException("JWT SigningKey is not configured.");
}

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey))
    };
});

var app = builder.Build();

async Task CreateRoles(IServiceProvider serviceProvider, ILogger logger)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    string[] roleNames = { "Administrator", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        logger.LogInformation("Checking if role exists: {Role} - Exists: {Exists}", roleName, roleExists);

        if (!roleExists)
        {
            logger.LogInformation("Creating role: {Role}", roleName);
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = configuration["Admin:AdminMail"];
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    logger.LogInformation("Checking if admin user exists: {Email} - Exists: {Exists}", adminEmail, adminUser != null);

    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            Name = "Admin",
            Surname = "User",
            DateJoined = DateTime.UtcNow,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, configuration["Admin:AdminPassword"]);

        if (result.Succeeded)
        {
            logger.LogInformation("Admin user created successfully.");
            await userManager.AddToRoleAsync(adminUser, "Administrator");
        }
        else
        {
            logger.LogError("Error creating admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";
    });

    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        await CreateRoles(services, logger);
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Enforce HTTPS
app.UseStaticFiles(); // Serve static files
app.UseRouting();  // Setup routing
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(   // Map attribute-routed controllers
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();
