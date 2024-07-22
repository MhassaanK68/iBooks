using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

ServiceProvider Provider = builder.Services.BuildServiceProvider();
var config = Provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<DBContext>(item => item.UseSqlServer(config.GetConnectionString("DBCS")));


builder.Services.AddSession();

var app = builder.Build();

 
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
} 

app.UseSession();   

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
