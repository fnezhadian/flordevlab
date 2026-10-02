using LabShop.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// ---- Added in Episode 1 ----
builder.Services.AddSingleton<Database>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Login");
// ----------------------------

var app = builder.Build();

// ---- Added in Episode 1: create the database and seed demo data ----
app.Services.GetRequiredService<Database>().Initialize();
// --------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // <-- added in Episode 1 (must come before UseAuthorization)
app.UseAuthorization();

app.MapRazorPages();

app.Run();
