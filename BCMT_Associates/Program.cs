using BCMT_Associates.Context;
using BCMT_Associates.Controllers;
using BCMT_Associates.Helpers;
using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using BCMT_Associates.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidAudience = builder.Configuration["Jwt:Audience"],
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/User/Login";
		options.Cookie.Name = "TokenCookieAuthentication";
	});
builder.Services.AddDbContext<AppDBContext>(Options =>
{
	Options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
});

builder.Services.Configure<OriginSettings>(builder.Configuration.GetSection("Origins"));

builder.Services.AddControllers(options =>
{
	// Apply ActivityLogFilter globally
	options.Filters.Add<ActivityLogFilter>();
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(1800);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserInterface, UserRepository>();
builder.Services.AddSingleton<IEmailService, EmailServiceRepository>();
builder.Services.AddSingleton<IMvcControllerDiscovery, MvcControllerDiscovery>();
builder.Services.AddScoped<IRolesInterface, RolesRepository>();
builder.Services.AddScoped<ActivityLogFilter>();

// Register other services
builder.Services.AddScoped<IActivityLogService, FileActivityLogService>();


var app = builder.Build();
app.UseCors("AllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
	context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
	context.Response.Headers.Add("Pragma", "no-cache");
	context.Response.Headers.Add("Expires", "0");

	await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();


app.UseCookiePolicy();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
