using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.ProductRepositories;
public class ProductDescriptionRepository_Tests
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
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Apple" });
        await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = "Samsung" });

        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "Svenska" });
        await _languageRepository.CreateAsync(new LanguageEntity() { Language = "English" });

        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Datorer", ParentCategory = 2 });
        await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = "Elektronik" });

        await _productRepository.CreateAsync(new ProductEntity()
        {
            ArticleNumber = "A1",
            Title = "iPhone 15",
            ManufacturerId = 1,
            Price = 10000,
            ProductCategoryId = 3,
        });

        await _descriptionRepository.CreateAsync(new ProductDescriptionEntity()
        {
            Description = "a",
            Specifications = "b",
            ArticleNumber = "A1",
            LanguageId = 1,
        });

        await _productRepository.CreateAsync(new ProductEntity()
        {
            ArticleNumber = "A2",
            Title = "Samsung Galaxy s28",
            ManufacturerId = 2,
            Price = 15000,
            ProductCategoryId = 3,
        });

        await _descriptionRepository.CreateAsync(new ProductDescriptionEntity()
        {
            Description = "c",
            Specifications = "d",
            ArticleNumber = "A2",
            LanguageId = 2,
        });

        // Act
        var result = await _descriptionRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 2);
    }

    [Fact]
    public async Task CreateDescriptionShould_CreateNewDescription_ThenReturnIt()
    {
        // Arrange
        var description = new ProductDescriptionEntity()
        {
            Description = "a",
            Specifications = "b",
            ArticleNumber = "A1",
            LanguageId = 1,
        };

        // Act
        var result = await _descriptionRepository.CreateAsync(description);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Description == "a");
    }

    [Fact]
    public async Task ReadDescriptionBy_ArticleNumberAndLanguageIdShould_FindDescriptions_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _descriptionRepository.ReadOneAsync("A1", 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Description == "a");
    }

    [Fact]
    public async Task ReadAllDescriptionsShould_RetrieveAllDescriptions_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _descriptionRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 2);
    }

    [Fact]
    public async Task UpdateDescriptionBy_ArticleNumberAndLanguageIdShould_FindAndUpdateTheDescription_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();
        var existingEntity = await _descriptionRepository.ReadOneAsync("A1", 1);

        // Act
        existingEntity.Description = "iPhone 15 Pro";
        var result = await _descriptionRepository.UpdateAsync("A1", 1, existingEntity);
        var descriptionList = await _descriptionRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Description == "iPhone 15 Pro");
        Assert.True(descriptionList.Count() == 2);
    }

    [Fact]
    public async Task DeleteDescriptionBy_ArticleNumberAndLanguageIdShould_FindAndDeleteProduct_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var descriptionToDelete = await _descriptionRepository.ReadOneAsync("A1", 1);

        // Act
        var descriptionDeleteResult = await _descriptionRepository.DeleteAsync("A1", 1, descriptionToDelete);
        var descriptionList = await _descriptionRepository.ReadAllAsync();

        // Assert
        Assert.True(descriptionDeleteResult);
        Assert.True(!descriptionList.Any(b => b.ArticleNumber == "A1" && b.LanguageId == 1));
        Assert.True(descriptionList.Count() == 1);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        await AddSampleData();

        // Act
        var entity = await _descriptionRepository.Existing(a => a.ArticleNumber == "A1");

        // Assert
        Assert.True(entity);
    }
}
