using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2CAJJ.Data;
using PracticaMvcCore2CAJJ.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();
// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("SqlLibros");
builder.Services.AddDbContext<LibrosContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<LibrosRepository>();
builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Libros}/{action=Index}/{id?}"
    );
});

app.Run();
