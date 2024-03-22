using MessageApplication.DbContext;
using MessageApplication.Models;
using MessageApplication.Service.Auth;
using MessageApplication.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MessageContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MessageContext")));
// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddIdentity<AppUser, IdentityRole<int>>(
    options => {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredLength = 5;
    }
    )
    .AddEntityFrameworkStores<MessageContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.LogoutPath = "/Auth/Logout";
    options.Cookie.Name = "MessageApplication";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});
builder.Services.AddTransient<IAuthService, AuthService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});
builder.Services.AddAuthentication(
   o=> { o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       }
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateTokenReplay = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:ValidIssuer").Value,
            ValidAudience = builder.Configuration.GetSection("Jwt:ValidAudiences").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
            .UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
           
        };
    });











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
