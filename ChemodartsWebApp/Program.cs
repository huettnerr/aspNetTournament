using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChemodartsWebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using ChemodartsWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string connectionString = builder.Configuration.GetConnectionString("ChemodartsContext") ?? throw new InvalidOperationException("Connection string 'ChemodarfsEFContext' not found.");
builder.Services.AddDbContext<ChemodartsContext>(
    options => options.UseLazyLoadingProxies().UseMySQL(connectionString), 
    ServiceLifetime.Scoped
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
    });

builder.Services.Configure<User>(builder.Configuration.GetSection("Admin"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "Tournament",
    pattern: "Tournament/{tournamentId?}/{action}/{id?}",
    defaults: new { controller = "Tournament", action = "Index"});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tournament}/{action=Index}/{id?}");

app.Run();
