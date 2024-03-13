using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;
 
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
builder.Services.AddSingleton<AuthMiddleware>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IGovtDepartmentRepository,GovtDepartmentRepository> ();
builder.Services.AddScoped<IGovtSchemeRepository, GovtSchemeRepository>();
builder.Services.AddScoped<IUserDetailRepository,UserDetailRepository>();
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDetailService, UserDetailService>();
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
      pattern: "{controller=Home}/{action=Index}/{id?}"
);



app.Run();