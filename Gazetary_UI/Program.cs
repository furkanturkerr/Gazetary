using BlogProject;
using Business.Abstract;
using Business.Concrate;
using DataAccess.Abstarct;
using DataAccess.Abstract;
using DataAccess.Concrate;
using DataAccess.EntityFramework;
using Entities.Concrate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("login-limit", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.AddPolicy("like-limit", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(10),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.AddPolicy("comment-limit", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3,
            Window = TimeSpan.FromSeconds(30),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.AddPolicy("register-limit", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.AddPolicy("verify-limit", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });
});

builder.Services.AddDbContext<Context>((_, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerConnection"));
});

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateManager>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpClient<IWeatherService, WeatherManager>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpClient<IRssService, RssManager>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddScoped<IImageDal, EfImageDal>();
builder.Services.AddScoped<IImageService, ImageManager>();

builder.Services.AddScoped<IBlogPostDal, EfBlogPostDal>();
builder.Services.AddScoped<IBlogPostService, BlogPostManager>();

builder.Services.AddScoped<ICommentDal, EfCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddScoped<ICommentLikeDal, EfCommentLikeDal>();
builder.Services.AddScoped<ICommentLikeService, CommentLikeManager>();

builder.Services.AddScoped<IContactDal, EfContactDal>();
builder.Services.AddScoped<IContactService, ContactManager>();

builder.Services.AddMemoryCache();

var turkishCulture = new System.Globalization.CultureInfo("tr-TR");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = turkishCulture;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = turkishCulture;

builder.Services.AddScoped<ISeoService, SeoManager>();

builder.Services.AddIdentity<AppUser, AppRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;

        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    options.AccessDeniedPath = "/";

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/Admin"))
        {
            context.Response.Redirect("/panel-9xk2-admin");
        }
        else
        {
            context.Response.Redirect("/UserLogin/Login");
        }

        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect("/");
        return Task.CompletedTask;
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<Context>();
        db.Database.Migrate();
        Log.Information("Database migration başarılı");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Database migration hatası - Uygulama başlatılamıyor!");
        throw;
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

            Log.Error(exception, "Unhandled exception");

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Sunucu hatası oluştu.");
        });
    });

    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseStatusCodePagesWithReExecute("/Error/404");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Anasayfa}/{id?}");

app.Run();