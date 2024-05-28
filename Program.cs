using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreadShare.Data;
using ThreadShare.Models;
using ThreadShare.Repository.Implementations;
using ThreadShare.Repository.Interfaces;
using ThreadShare.Service.Implementations;
using ThreadShare.Service.Interfaces;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options=>options.UseNpgsql
                (configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});


// Myb change these 
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IForumRepository, ForumRepository>();
builder.Services.AddScoped<IRepository<Comment>, CommentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IForumService, ForumService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddAuthentication();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

builder.Services.AddScoped<ITokenService, TokenService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();  
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();