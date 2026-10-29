using LabShop.Data;
using LabShop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;   // NEW (Ep 3)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<PasswordService>();

// NEW (Ep 3): EF Core replaces the hand-written Database class.
// Same SQLite file as before, same relative path.
builder.Services.AddDbContext<LabDbContext>(options =>
    options.UseSqlite("Data Source=labshop.db"));

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Login");

var app = builder.Build();

// NEW (Ep 3): bring the database up to date from the migrations, then seed demo data once.
// An existing database needs its first migration marked as applied (see scripts\04-baseline-ef-history.ps1).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LabDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db, scope.ServiceProvider.GetRequiredService<PasswordService>());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
