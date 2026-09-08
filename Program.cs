using System.IO;
using Microsoft.EntityFrameworkCore;
using SistemaHotelaria.Services;
using SistemaHotelaria.Services.Facade;
using SistemaHotelaria.Services.Notifications;
using SistemaHotelaria.Services.Observer;
using SistemaHotelaria.Services.Persistence;
using SistemaHotelaria.Services.Proxies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "hotelaria.db");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<IReservaRepository, EntityFrameworkReservaRepository>();
builder.Services.AddSingleton<IInventarioQuartos, InMemoryInventarioQuartosAdapter>();
builder.Services.AddSingleton<ReservaNotificacaoAdapter>();
builder.Services.AddSingleton<INotificacaoReserva>(_ =>
    new CompositeNotificacaoAdapter(
        new ConsoleNotificacaoAdapter(),
        new WebNotificacaoAdapter()));

builder.Services.AddSingleton<IObserver, ServicoEmail>();
builder.Services.AddSingleton<IObserver, ServicoLimpeza>();
builder.Services.AddSingleton<IObserver, Recepcao>();

builder.Services.AddScoped<GerenciadorReservas>();

builder.Services.AddSingleton<IGerenciadorReservas>(sp =>
{
    var real = sp.GetRequiredService<GerenciadorReservas>();
    var logger = sp.GetRequiredService<ILogger<GerenciadorReservasProxy>>();
    var comValidacao = new GerenciadorReservasProxy(real, logger);
    return new CachingDisponibilidadeProxy(comValidacao);
});

builder.Services.AddSingleton<HotelService>();

builder.Services.AddSingleton<IReservaFacade, ReservaFacade>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    GarantirColunasPagamentoEObserver(db);
}

static void GarantirColunasPagamentoEObserver(ApplicationDbContext db)
{
    string[] sqls =
    [
        "ALTER TABLE Reservas ADD COLUMN PagamentoTransacaoId TEXT DEFAULT ''",
        "ALTER TABLE Reservas ADD COLUMN PagamentoComprovante TEXT DEFAULT ''",
        "ALTER TABLE Reservas ADD COLUMN StatusReserva TEXT DEFAULT 'Confirmada'",
        "ALTER TABLE Reservas ADD COLUMN EventosReserva TEXT DEFAULT ''"
    ];

    foreach (var sql in sqls)
    {
        try
        {
            db.Database.ExecuteSqlRaw(sql);
        }
        catch
        {
            // Coluna já existe no banco atual.
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
