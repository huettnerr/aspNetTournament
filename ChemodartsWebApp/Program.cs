using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChemodartsWebApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Web.Mvc;

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

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

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

//Monitor Controller
//app.MapControllerRoute(
//    name: "Monitor",
//    pattern: "Monitor/{action}",
//    defaults: new { controller = "Monitor", action = "Index"});

//Tournament Controller
app.MapControllerRoute(
    name: "Tournament",
    pattern: "Tournament/{action}/{tournamentId:int?}",
    defaults: new { controller = "Tournament", action = "Index"});

//Round Controller
app.MapControllerRoute(
    name: "Round",
    pattern: "Tournament/{tournamentId}/Round/{action}/{roundId:int?}",
    defaults: new { controller = "Round", action = "Index"});

//Seed Controller
app.MapControllerRoute(
    name: "Seed",
    pattern: "Tournament/{tournamentId}/Seed/{action}/{seedId:int?}",
    defaults: new { controller = "Seed", action = "Index"});

//Group Controller
app.MapControllerRoute(
    name: "Group",
    pattern: "Tournament/{tournamentId}/Round/{roundId}/Group/{action}/{groupId:int?}",
    defaults: new { controller = "Group", action = "Index" });

//Round Controller
app.MapControllerRoute(
    name: "Match",
    pattern: "Tournament/{tournamentId}/Round/{roundId}/Match/{action}/{matchId:int?}",
    defaults: new { controller = "Match", action = "Index" });

//Round Controller
app.MapControllerRoute(
    name: "Venue",
    pattern: "Tournament/{tournamentId}/Round/{roundId}/Venue/{action}/{venueId:int?}",
    defaults: new { controller = "Venue", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
