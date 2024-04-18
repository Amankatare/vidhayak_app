using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Services;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;
using VidhayakApp.Web.MiddleWare;

//write the session service just below the build line as shown
//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSession();

//var app = builder.Build();

//app.UseSession();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuration
builder.Configuration.AddJsonFile("appsettings.json");

//MySQL Connection :-
builder.Services.AddDbContext<VidhayakAppContext>(options =>
       options.UseMySQL(builder.Configuration.GetConnectionString("VidhayakAppConnection") // ,
                                                                                           //new MySqlServerVersion(new Version(8, 0, 2)))); // Adjust the version accordingly 
                                                                                           // MySql version 8.0.27 
));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthMiddleware>();
builder.Services.AddTransient<LoginMiddleware>();
builder.Services.AddTransient<RegisterMiddleware>();
builder.Services.AddTransient<AdminMiddleware>();
builder.Services.AddTransient<AppUserMiddleware>();
builder.Services.AddTransient<UserMiddleware>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IGovtDepartmentRepository,GovtDepartmentRepository> ();
builder.Services.AddScoped<IGovtSchemeRepository, GovtSchemeRepository>();
builder.Services.AddScoped<IWardRepository, WardRepository>();
builder.Services.AddScoped<IUserDetailRepository,UserDetailRepository>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
        });

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(o =>
//    {
//        // o.Authority = "VidhayakApp";
//        o.TokenValidationParameters = new TokenValidationParameters
//        {

//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey
//            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true, // token's expiration
//            ValidateIssuerSigningKey = true
//        };
//    });

    builder.Services.AddAuthorization(options =>
    {
      
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

  
        options.AddPolicy("RequireAppUserRole", policy => policy.RequireRole("AppUser"));

     
        options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));

       
    });



//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization();



builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

app.UseSession();
//app.UseMiddleware<AuthMiddleware>();
//app.UseMiddleware<AuthMiddleware>();

// Route for LoginMiddleware
app.MapWhen(context => context.Request.Path.StartsWithSegments("/Account/Login"), appBuilder =>
{
    appBuilder.UseMiddleware<LoginMiddleware>();
});

// Route for RegisterMiddleware
app.MapWhen(context => context.Request.Path.StartsWithSegments("/Account/Register"), appBuilder =>
{
    appBuilder.UseMiddleware<RegisterMiddleware>();
});

// Route for AdminMiddleware
app.MapWhen(context => context.Request.Path.StartsWithSegments("/Admin/"), appBuilder =>
{
    appBuilder.UseMiddleware<AdminMiddleware>();
});
app.MapWhen(context => context.Request.Path.StartsWithSegments("/AppUser/"), appBuilder =>
{
    appBuilder.UseMiddleware<AppUserMiddleware>();
});
app.MapWhen(context => context.Request.Path.StartsWithSegments("/User/"), appBuilder =>
{
    appBuilder.UseMiddleware<UserMiddleware>();
});

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


// Admin routes
app.MapControllerRoute(
    name: "AdminDashboard",
    pattern: "/Admin/Dashboard", // Unique URL pattern for Admin's dashboard
    defaults: new { controller = "Admin", action = "Dashboard" });

// AppUser routes
app.MapControllerRoute(
    name: "AppUserDashboard",
    pattern: "/AppUser/Dashboard", // Unique URL pattern for AppUser's dashboard
    defaults: new { controller = "AppUser", action = "Dashboard" });

// User routes
app.MapControllerRoute(
    name: "UserDashboard",
    pattern: "/User/Dashboard", // Unique URL pattern for User's dashboard
    defaults: new { controller = "User", action = "Dashboard" });











// Admin routes
//app.MapControllerRoute(
//                    name: "Admin",
//                    pattern: "Dashboard", // Define your desired URL pattern here
//                    defaults: new { controller = "Admin", action = "Dashboard" } // This will call Dashboard method in AdminController
//                );
//app.MapControllerRoute(
//                    name: "AppUser",
//                    pattern: "Dashboard", // Define your desired URL pattern here
//                    defaults: new { controller = "AppUser", action = "Dashboard" } // This will call Dashboard method in AdminController
//                );
//app.MapControllerRoute(
//                    name: "User",
//                    pattern: "Dashboard", // Define your desired URL pattern here
//                    defaults: new { controller = "User", action = "Dashboard" } // This will call Dashboard method in AdminController
//                );
//app.MapControllerRoute(
//                    name: "Admin",
//                    pattern: "Dashboard", // Define your desired URL pattern here
//                    defaults: new { controller = "Admin", action = "Dashboard" } // This will call Dashboard method in AdminController
//                );
// AppUser routes
//endpoints.MapControllerRoute(
//    name: "AppUserDashboard",
//    pattern: "AppUser/Dashboard",
//    defaults: new { controller = "AppUser", action = "Dashboard" });

//// User routes
//endpoints.MapControllerRoute(
//    name: "UserDashboard",
//    pattern: "User/Dashboard",
//    defaults: new { controller = "User", action = "Dashboard" });

// Login route
app.MapControllerRoute(
        name: "Login",
        pattern: "Login",
        defaults: new { controller = "Account", action = "Login" });

    // Registration route
    app.MapControllerRoute(
        name: "Register",
        pattern: "Register",
        defaults: new { controller = "Account", action = "Register" });

    // Default route
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseHttpsRedirection();
app.UseStaticFiles();


//app.MapControllerRoute(
//      name: "default",
//      pattern: "{controller=Home}/{action=Index}/{id?}"
//);



app.Run();