using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TendalProject.Business.Interfaces;
using TendalProject.Business.Interfaeces;
using TendalProject.Business.Services;
using TendalProject.Common.Time;
using TendalProject.Data.Context;
using TendalProject.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IArticuloService, ArticuloService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IEcommerceService, EcommerceService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

//SecretsKey
builder.Services.AddScoped<IPagoService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>(); 
    var uow = sp.GetRequiredService<IUnitOfWork>();
    var dateTimeProvider = sp.GetRequiredService<IDateTimeProvider>();
    return new PagoService(config, uow, dateTimeProvider);
});

//AppDbContext para EntityFramework
var cn1 = builder.Configuration.GetConnectionString("cn1");
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(cn1));

//Authentication Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";     
        options.LogoutPath = "/Auth/Logout";   
        options.AccessDeniedPath = "/Auth/AccessDenied";
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
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ecommerce}/{action=Catalogo}/{id?}")
    .WithStaticAssets();
app.Run();
