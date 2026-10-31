using LabShop.Data;
using LabShop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;       // NEW (Ep 5)
using Microsoft.AspNetCore.RateLimiting;    // NEW (Ep 5)

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

// NEW (Ep 5): a rate limiter that is already part of ASP.NET Core. No new package.
// One policy called "login": a fixed window, 3 tries per minute for each client address.
// It counts only POST requests (the sign-in tries). Opening the page is not counted.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // When the limit is reached: answer 429 and say how many seconds to wait.
    options.OnRejected = async (context, cancellationToken) =>
    {
        var seconds = 60;
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            seconds = (int)Math.Ceiling(retryAfter.TotalSeconds);
            context.HttpContext.Response.Headers["Retry-After"] = seconds.ToString();
        }
        context.HttpContext.Response.ContentType = "text/plain";
        await context.HttpContext.Response.WriteAsync(
            $"429, Too Many Requests. Try again in {seconds} seconds.", cancellationToken);
    };

    options.AddPolicy("login", httpContext =>
        httpContext.Request.Method == HttpMethods.Post
            ? RateLimitPartition.GetFixedWindowLimiter(
                httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 3,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                })
            : RateLimitPartition.GetNoLimiter("not-a-sign-in-try"));
});

var app = builder.Build();

// NEW (Ep 3): bring the database up to date from the migrations, then seed demo data once.
// An existing database needs its first migration marked as applied (see the baseline step in the production guide).
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

// NEW (Ep 5): turn the rate limiter on (after routing, so the page rule is known).
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
