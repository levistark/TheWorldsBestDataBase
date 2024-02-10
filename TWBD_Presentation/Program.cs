using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TWBD_Domain.Services;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Repositories;
using TWBD_Presentation.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
        {
            services.AddDbContext<UserDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\VSProjects\Datalagring\TheWorldsBestDataBase\TWBD_Infrastructure\Data\users_db.mdf;Integrated Security=True;Connect Timeout=30"));
            services.AddDbContext<ProductDataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\VSProjects\Datalagring\TheWorldsBestDataBase\TWBD_Infrastructure\Data\products_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"));

            services.AddScoped<RoleRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<AuthenticationRepository>();
            services.AddScoped<ProfileRepository>();
            services.AddScoped<AddressRepository>();

            services.AddScoped<LanguageRepository>();
            services.AddScoped<ManufacturerRepository>();
            services.AddScoped<ProductCategoryRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductDescriptionRepository>();
            services.AddScoped<ProductReviewRepository>();

            services.AddScoped<MenuService>();

            services.AddScoped<UserService>();
            services.AddScoped<UserAddressService>();
            services.AddScoped<UserRoleService>();
            services.AddScoped<UserSecurityService>();
            services.AddScoped<UserValidationService>();
            services.AddScoped<UserLoginService>();

            services.AddScoped<LanguageService>();
            services.AddScoped<ManufacturerService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<ProductService>();
            services.AddScoped<DescriptionService>();
            services.AddScoped<ReviewService>();
            services.AddScoped<ProductValidationService>();

            services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Error);
                builder.AddFilter("System", LogLevel.Error);
            });

        }).Build();

        builder.Start();
        Console.Clear();

        var _menuService = builder.Services.GetRequiredService<MenuService>();
        await _menuService.StartMenu();
    }
}