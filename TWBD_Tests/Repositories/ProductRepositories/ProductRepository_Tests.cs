using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.ProductRepositories;
public class ProductRepository_Tests
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
        var result = await _productRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 2);
    }

    [Fact]
    public async Task CreateProductShould_CreateNewProduct_ThenReturnIt()
    {
        // Arrange
        var product = new ProductEntity()
        {
            ArticleNumber = "A1",
            Title = "iPhone 15",
            ManufacturerId = 1,
            Price = 10000,
            ProductCategoryId = 3,
        };

        // Act
        var result = await _productRepository.CreateAsync(product);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ManufacturerId == 1);
    }

    [Fact]
    public async Task ReadOneProductByArticleNumberShould_FindProduct_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _productRepository.ReadOneAsync(x => x.ArticleNumber == "A1");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Title == "iPhone 15");
    }

    [Fact]
    public async Task ReadOneProductByTitleShould_FindManufacturer_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _productRepository.ReadOneAsync(x => x.Title == "iPhone 15");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ArticleNumber == "A1");
    }

    [Fact]
    public async Task ReadAllProductsShould_RetrieveAllProducts_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _productRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 2);
    }

    [Fact]
    public async Task UpdateProductByArticleNumberShould_FindAndUpdateTheProduct_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();
        var existingEntity = await _productRepository.ReadOneAsync(x => x.ArticleNumber == "A1");

        // Act
        existingEntity.Title = "iPhone 15 Pro";
        var result = await _productRepository.UpdateAsync(x => x.ArticleNumber == "A1", existingEntity);
        var productList = await _productRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Title == "iPhone 15 Pro");
        Assert.True(productList.Count() == 2);
    }

    [Fact]
    public async Task DeleteProductByArticleNumberShould_FindAndDeleteProduct_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var productToDelete = await _productRepository.ReadOneAsync(a => a.ArticleNumber == "A1");
        var descriptionToDelete = await _descriptionRepository.ReadOneAsync(desc => desc.ArticleNumber == "A1");

        // Act
        var descriptionDeleteResult = await _descriptionRepository.DeleteAsync(d => d.ArticleNumber == "A1", descriptionToDelete);
        var productDeleteResult = await _productRepository.DeleteAsync(x => x.ArticleNumber == "A1", productToDelete);
        var productList = await _productRepository.ReadAllAsync();

        // Assert
        Assert.True(descriptionDeleteResult);
        Assert.True(!productList.Any(b => b.ArticleNumber == "A1"));
        Assert.True(!productList.Any(b => b.ManufacturerId == 1));
        Assert.True(productList.Count() == 1);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        await AddSampleData();

        // Act
        var entity = await _productRepository.Existing(a => a.ArticleNumber == "A1");

        // Assert
        Assert.True(entity);
    }
}
