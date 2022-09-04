using AutoMapper;
using ForumWebsite.Mappings;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.Context;
using ForumWebsite.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .SetBasePath(Directory.GetCurrentDirectory())
    .Build();

builder.Services.AddDbContext<ForumDbContext>(options =>
{
    options.UseMySql(configuration.GetValue<string>("ConnectionStrings:DefaultConnection"), ServerVersion.Parse("8.0.29-mysql"));
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;

    options.SignIn.RequireConfirmedEmail = false;

    options.User.RequireUniqueEmail = true;
}).AddPasswordValidator<PasswordValidation>()
.AddUserValidator<UserValidation>()
.AddEntityFrameworkStores<ForumDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.LogoutPath = "/User/Logout";
    options.AccessDeniedPath = "/User/AccessDenied";

    options.Cookie = new CookieBuilder()
    {
        Name = "ForumWebsiteCookie",
        HttpOnly = false,
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.Always
    };

    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

var autoMapper = new MapperConfiguration(options =>
{
    options.AddProfile(new AuthenticationMapping());
});

builder.Services.AddSingleton(autoMapper.CreateMapper());

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
