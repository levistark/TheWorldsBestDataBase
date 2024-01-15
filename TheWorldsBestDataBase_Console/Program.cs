using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWorldsBestDataBase_Console;

// Instansiera klasser och services
var app = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddSingleton<ViewServices>();
}).Build();

app.Start();
Console.Clear();

var viewServices = app.Services.GetRequiredService<ViewServices>();

// Ladda upp start-menyn
viewServices.StartMenuView();
