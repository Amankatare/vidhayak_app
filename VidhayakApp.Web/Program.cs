using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;

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
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
