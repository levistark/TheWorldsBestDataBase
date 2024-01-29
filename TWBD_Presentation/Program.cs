using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWBD_Infrastructure.Contexts;
using TWBD_Presentation.Services;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<UserDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\VSProjects\Datalagring\TheWorldsBestDataBase\TWBD_Infrastructure\Data\users_db.mdf;Integrated Security=True;Connect Timeout=30"));
    services.AddSingleton<MenuService>();
}).Build();

builder.Start();
Console.Clear();

var _menuService = builder.Services.GetRequiredService<MenuService>();
_menuService.StartMenu();
