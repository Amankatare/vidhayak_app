using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Services;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;
using VidhayakApp.Web.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<VidhayakAppContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("VidhayakAppConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthMiddleware>();
builder.Services.AddTransient<LoginMiddleware>();
builder.Services.AddTransient<RegisterMiddleware>();
builder.Services.AddTransient<AdminMiddleware>();
builder.Services.AddTransient<AppUserMiddleware>();
builder.Services.AddTransient<UserMiddleware>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGovtDepartmentRepository, GovtDepartmentRepository>();
builder.Services.AddScoped<IGovtSchemeRepository, GovtSchemeRepository>();
builder.Services.AddScoped<IWardRepository, WardRepository>();
builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireAppUserRole", policy => policy.RequireRole("AppUser"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});

//app.Map("/ping", app2 => app2.Run(async context =>
//{
//    await context.Response.WriteAsync("Pong");
//}));

//app.MapControllerRoute(
//    name: "/Admin/Dashboard",
//    pattern: "/Admin/Dashboard",
//    defaults: new { controller = "Admin", action = "Dashboard" });

//app.MapControllerRoute(
//    name: "AppUserDashboard",
//    pattern: "/AppUser/Dashboard",
//    defaults: new { controller = "AppUser", action = "Dashboard" });

//app.MapControllerRoute(
//    name: "UserDashboard",
//    pattern: "/User/Dashboard",
//    defaults: new { controller = "User", action = "Dashboard" });

//app.MapControllerRoute(
//    name: "Login",
//    pattern: "Login",
//    defaults: new { controller = "Account", action = "Login" });

//app.MapControllerRoute(
//    name: "Register",
//    pattern: "Register",
//    defaults: new { controller = "Account", action = "Register" });

app.


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
