using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChemodartsWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string connectionString = builder.Configuration.GetConnectionString("ChemodartsContext") ?? throw new InvalidOperationException("Connection string 'ChemodarfsEFContext' not found.");
builder.Services.AddDbContext<ChemodartsContext>(
    options => options.UseLazyLoadingProxies().UseMySQL(connectionString), 
    ServiceLifetime.Scoped
);

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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
