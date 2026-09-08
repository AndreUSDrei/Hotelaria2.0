using SistemaHotelaria.Services.Notifications;
using SistemaHotelaria.Services.Observer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register Composite notification service
builder.Services.AddSingleton<INotificacaoReserva>(sp =>
{
    var composite = new CompositeNotificacaoAdapter();
    composite.Adicionar(new ConsoleNotificacaoAdapter());
    composite.Adicionar(new WebNotificacaoAdapter());
    return composite;
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
