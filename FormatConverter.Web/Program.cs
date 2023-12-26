using FormatConverter.Web.Data;
using FormatConverter.Web.Infrastructure;
using FormatConverter.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    // Configure password rules
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IZipService, ZipService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "CookieAuthentication";
});
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Authentication";
});

var app = builder.Build();

// Singleton for seeding the database. CreateScope() method creates a room for a singleton to use the scoped services.
using (IServiceScope scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetService<ILogger<DataSeeder>>();
    var dataSeeder = scope.ServiceProvider.GetService<DataSeeder>();
    try
    {
        logger?.LogInformation("Preparing the database");
        dataSeeder?.SeedAsync().Wait();
    }
    catch (Exception exception)
    {
        logger?.LogError(exception, "Failed to seed the database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
