using LabShop.Data;
using LabShop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<Database>();
builder.Services.AddSingleton<PasswordService>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Login");

var app = builder.Build();

app.Services.GetRequiredService<Database>().Initialize();

// NEW (Ep 2): one-time migration.  Run it with:  dotnet run -- migrate-passwords
if (args.Contains("migrate-passwords"))
{
    var migrated = PasswordMigration.Run(
        app.Services.GetRequiredService<Database>(),
        app.Services.GetRequiredService<PasswordService>());

    Console.WriteLine($"Migrated {migrated} user(s): plain text passwords are now hashes.");
    return;
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
