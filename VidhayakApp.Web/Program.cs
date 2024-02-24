using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;
using VidhayakApp.Web.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json");

//MySQL Connection :-
builder.Services.AddDbContext<VidhayakAppContext>(options =>
       options.UseMySQL(builder.Configuration.GetConnectionString("VidhayakAppConnection") // ,
       //new MySqlServerVersion(new Version(8, 0, 2)))); // Adjust the version accordingly 
       // MySql version 8.0.27 
));

//builder.Services.Configure<AppConfig>(Configuration.GetSection("VidhayakAppConnection"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
       // o.Authority = "VidhayakApp";
        o.TokenValidationParameters = new TokenValidationParameters
        {
            
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // token's expiration
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.UseMiddleware<RoleBasedAuthenticationMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
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
