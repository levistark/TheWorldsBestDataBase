using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.ProductRepositories;
public class ProductReviewRepository_Tests
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

        await _reviewRepository.CreateAsync(new ProductReviewEntity()
        {
            Rating = 5,
            ArticleNumber = "A1"
        });

        await _reviewRepository.CreateAsync(new ProductReviewEntity()
        {
            Rating = 1,
            ArticleNumber = "A1"
        });

        await _reviewRepository.CreateAsync(new ProductReviewEntity()
        {
            Rating = 10,
            ArticleNumber = "A2"
        });

        await _reviewRepository.CreateAsync(new ProductReviewEntity()
        {
            Rating = 1,
            ArticleNumber = "A2"
        });

        // Act
        var result = await _reviewRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 4);
    }

    [Fact]
    public async Task CreateReviewShould_CreateNewReview_ThenReturnIt()
    {
        // Arrange
        var review = new ProductReviewEntity()
        {
            Rating = 1,
            ArticleNumber = "A1",
        };

        // Act
        var result = await _reviewRepository.CreateAsync(review);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Rating == 1);
    }

    [Fact]
    public async Task ReadReviewByIdShould_FindReview_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _reviewRepository.ReadOneAsync(x => x.Id == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Rating == 5);
    }

    [Fact]
    public async Task ReadAllReviewsShould_RetrieveAllReviews_ThenReturnList()
    {
        // Arrange
        await AddSampleData();

        // Act
        var result = await _reviewRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 4);
    }

    [Fact]
    public async Task UpdateReviewByIdShould_FindAndUpdateReview_ThenReturnIt()
    {
        // Arrange
        await AddSampleData();
        var existingEntity = await _reviewRepository.ReadOneAsync(r => r.Id == 1);

        // Act
        existingEntity.Rating = 9;
        var result = await _reviewRepository.UpdateAsync(x => x.Id == 1, existingEntity);
        var reviewList = await _reviewRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Rating == 9);
        Assert.True(reviewList.Count() == 4);
    }

    [Fact]
    public async Task DeleteReviewByIdShould_FindAndDeleteReview_ThenReturnTrue()
    {
        // Arrange
        await AddSampleData();
        var reviewToDelete = await _reviewRepository.ReadOneAsync(r => r.Id == 1);

        // Act
        var result = await _reviewRepository.DeleteAsync(x => x.Id == 1, reviewToDelete);
        var reviewList = await _reviewRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!reviewList.Any(b => b.Id == 1));
        Assert.True(reviewList.Count() == 3);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        await AddSampleData();

        // Act
        var entity = await _reviewRepository.Existing(a => a.Id == 1);

        // Assert
        Assert.True(entity);
    }
}
