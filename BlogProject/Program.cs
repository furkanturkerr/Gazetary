using Business.Abstract;
using Business.Concrate;
using DataAccess.Abstarct;
using DataAccess.Abstract;
using DataAccess.Concrate;
using DataAccess.EntityFramework;
using Entities.Concrate;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Context>();

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<IBlogPostDal, EfBlogPostDal>();
builder.Services.AddScoped<IBlogPostService, BlogPostManager>();

builder.Services.AddScoped<ICommentDal, EfCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddScoped<ICommentLikeDal, EfCommentLikeDal>();
builder.Services.AddScoped<ICommentLikeService, CommentLikeManager>();

builder.Services.AddScoped<IContactDal, EfContactDal>();
builder.Services.AddScoped<IContactService, ContactManager>();

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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

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