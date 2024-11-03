using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Implementations;
using ThreadShare.Repository.Interfaces;
using ThreadShare.Service.Implementations;
using ThreadShare.Service.Interfaces;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql
                (configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});


// Change these
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
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

        var result = await userManager.CreateAsync(adminUser, configuration["Admin:AdminPass"]);

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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    await CreateRoles(services, logger);
}

app.Run();
