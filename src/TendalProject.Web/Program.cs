using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TendalProject.Business.Interfaces;
using TendalProject.Business.Interfaeces;
using TendalProject.Business.Services;
using TendalProject.Common.Time;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Data.Repositories;
using TendalProject.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

//Repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();


//AppDbContext para EntityFramework
var cn1 = builder.Configuration.GetConnectionString("cn1");
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(cn1));

//Authentication Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";     // Página de login
        options.LogoutPath = "/Auth/Logout";   // Página de logout
        options.AccessDeniedPath = "/Auth/Denegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//Aplicar migraciones pendientes
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseRouting();

//Authenticación y authorizacion
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
