using Microsoft.EntityFrameworkCore;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class ProductValidationService_Tests
{

    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ProductRepository _productRepository = new(_productDataContext);
    private readonly ManufacturerRepository _manufacturerRepository = new(_productDataContext);
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
    public async Task ValidateArticleNumberShould_CheckIfExisting_ThenReturnTrue()
    {
        // Arrange
        ProductValidationService _validationService = new ProductValidationService(_productRepository);
        await AddSampleData();

        // Act
        var result = await _validationService.ValidateArticleNumber("A6");

        // Assert
        Assert.True(result);
    }
}
