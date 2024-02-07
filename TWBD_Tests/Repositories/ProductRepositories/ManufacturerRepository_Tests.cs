using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.ProductRepositories;
public class ManufacturerRepository_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ManufacturerRepository _manufacturerRepository = new(_productDataContext);
    private readonly ProductRepository _productRepository = new(_productDataContext);
    private readonly LanguageRepository _languageRepository = new(_productDataContext);
    private readonly ProductCategoryRepository _categoryRepository = new(_productDataContext);
    private readonly ProductReviewRepository _reviewRepository = new(_productDataContext);
    private readonly ProductDescriptionRepository _descriptionRepository = new(_productDataContext);

    [Fact]
    public async Task AddSampleData()
    {
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Apple"});
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Samsung" });
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Microsoft" });

        // Act
        var result = await _manufacturerRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async Task CreateManufacturerShould_CreateNewManufacturer_ThenReturnIt()
    {
        // Arrange
        var manufacturer = new ManufacturerEntity() { Manufacturer = "Apple", };

        // Act
        var result = await _manufacturerRepository.CreateAsync(manufacturer);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Manufacturer == "Apple");
    }

    [Fact]
    public async Task ReadOneManufacturerByIdShould_FindManufacturer_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _manufacturerRepository.ReadOneAsync(x => x.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Manufacturer == "Apple");
    }

    [Fact]
    public async Task ReadOneManufacturerByNameShould_FindManufacturer_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _manufacturerRepository.ReadOneAsync(x => x.Manufacturer == "Apple");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id == 1);
    }

    [Fact]
    public async Task ReadAllManufacturersShould_RetrieveAllManufacturers_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _manufacturerRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
        Assert.True(result.Count() == 3);
    }

    [Fact]
    public async Task UpdateManufacturerByIdShould_FindAndUpdateTheManufacturer_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();
        var existingCategory = await _manufacturerRepository.ReadOneAsync(x => x.Id == 1);

        // Act
        existingCategory.Manufacturer = "Apples";
        var result = await _manufacturerRepository.UpdateAsync(x => x.Id == 1, existingCategory);
        var categoryList = await _manufacturerRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Manufacturer == "Apples");
        Assert.True(categoryList.Count() == 3);
    }

    [Fact]
    public async Task DeleteManufacturerByIdShould_FindAndDeleteManufacturer_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var entityToDelete = await _manufacturerRepository.ReadOneAsync(a => a.Id == 1);

        // Act
        var result = await _manufacturerRepository.DeleteAsync(x => x.Id == 1, entityToDelete);
        var categoryList = await _manufacturerRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!categoryList.Any(b => b.Id == 1));
        Assert.True(!categoryList.Any(b => b.Manufacturer == "Apple"));
        Assert.True(categoryList.Count() == 2);
    }

    [Fact]
    public async Task DeleteManufacturerByNameShould_FindAndDeleteManufacturer_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var entityToDelete = await _manufacturerRepository.ReadOneAsync(a => a.Manufacturer == "Apple");

        // Act
        var result = await _manufacturerRepository.DeleteAsync(x => x.Manufacturer == "Apple", entityToDelete);
        var categoryList = await _manufacturerRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!categoryList.Any(b => b.Id == 1));
        Assert.True(!categoryList.Any(b => b.Manufacturer == "Apple"));
        Assert.True(categoryList.Count() == 2);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        await AddSampleData();

        // Act
        var entity = await _manufacturerRepository.Existing(a => a.Id == 1);

        // Assert
        Assert.True(entity);
    }
}
