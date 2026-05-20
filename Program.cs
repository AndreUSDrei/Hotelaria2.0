using SistemaHotelaria.Services;
using SistemaHotelaria.Services.Facade;
using SistemaHotelaria.Services.Notifications;
using SistemaHotelaria.Services.Persistence;
using SistemaHotelaria.Services.Proxies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddLogging();

// Adapter: persistência, inventário e notificações (todos os adapters registrados)
builder.Services.AddSingleton<IReservaRepository, InMemoryReservaRepository>();
builder.Services.AddSingleton<IInventarioQuartos, InMemoryInventarioQuartosAdapter>();
builder.Services.AddSingleton<ReservaNotificacaoAdapter>();
builder.Services.AddSingleton<INotificacaoReserva>(_ =>
    new CompositeNotificacaoAdapter(
        new ConsoleNotificacaoAdapter(),
        new WebNotificacaoAdapter()));

// Serviço real de reservas
builder.Services.AddSingleton<GerenciadorReservas>();

// Proxy: validação/log → cache de disponibilidade
builder.Services.AddSingleton<IGerenciadorReservas>(sp =>
{
    var real = sp.GetRequiredService<GerenciadorReservas>();
    var logger = sp.GetRequiredService<ILogger<GerenciadorReservasProxy>>();
    var comValidacao = new GerenciadorReservasProxy(real, logger);
    return new CachingDisponibilidadeProxy(comValidacao);
});

builder.Services.AddSingleton<HotelService>();

// Facade: único ponto de entrada dos controllers
builder.Services.AddSingleton<IReservaFacade, ReservaFacade>();

var app = builder.Build();

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
