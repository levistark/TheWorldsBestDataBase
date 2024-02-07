using Microsoft.EntityFrameworkCore;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class ManufacturerService_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ManufacturerRepository _manufacturerRepository = new(_productDataContext);

    private async void AddSampleData()
    {
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Apple" });
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Samsung" });
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Microsoft" });
    }

    [Fact]
    public async Task GetManufacturerIdShould_FindManufacturerName_ThenReturnIt()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        AddSampleData();

        // Act
        var result = await _manufacturerService.GetManufacturerId("Apple");

        // Assert
        Assert.True(result == 1);
    }

    [Fact]
    public async Task GetManufacturerByIdShould_FindManufacturerName_ThenReturnIt()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        AddSampleData();

        // Act
        var result = await _manufacturerService.GetManufacturerById(1);

        // Assert
        Assert.True(result == "Apple");
    }
}
