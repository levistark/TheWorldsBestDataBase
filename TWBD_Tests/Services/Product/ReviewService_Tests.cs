using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class ReviewService_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ProductReviewRepository _reviewRepository = new(_productDataContext);

    [Fact]
    public async Task<int> AddSampleData()
    {
        ReviewService _productRepository = new ReviewService(_reviewRepository);

        var review1 = await _reviewRepository.CreateAsync(new ProductReviewEntity()
        {
            Review = "bäst",
            Rating = 10,
            Author = "Levi",
            ArticleNumberNavigation = new ProductEntity()
            {
                ArticleNumber = "A1",
                Title = "iPhone",
                Manufacturer = new ManufacturerEntity()
                {
                    Manufacturer = "Apple"
                },
                ManufacturerId = 1,
                ProductCategory = new ProductCategoryEntity()
                {
                    Category = "Cellphones",
                    ParentCategory = 0,
                },
                ProductCategoryId = 1,
                Price = 10,
            },
            ArticleNumber = "A1",
        });

        var review2 = await _reviewRepository.CreateAsync(new ProductReviewEntity()
        {
            Review = "sämst",
            Rating = 1,
            Author = "Patrik",
            ArticleNumber = "A1",
        });

        var reviewList = await _reviewRepository.ReadAllAsync();

        Assert.NotNull(reviewList);
        Assert.True(reviewList.Any());
        Assert.Contains(reviewList, x => x.ArticleNumber == "A1");
        Assert.True(reviewList.Count() == 2);

        return review1.Id;
    }

    [Fact]
    public async Task CreateReviewShould_AddNewReviewToDb_ThenReturnItAsModel()
    {
        // Arrange
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        await AddSampleData();

        // Act
        var result = await _reviewService.CreateReview(new ReviewModel()
        {
            Review = "SÄMST",
            Rating = 0,
            Author = "KUNDEN",
            ArticleNumber = "A1"
        });

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Author == "KUNDEN");
    }

    [Fact]
    public async Task CreateReviewShould_AddNewBadReviewToDb_ThenReturnNull()
    {
        // Arrange
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        await AddSampleData();

        // Act
        var result = await _reviewService.CreateReview(new ReviewModel()
        {
            Review = "SÄMST",
            Rating = 0,
            Author = "Levi",
            ArticleNumber = "A1"
        });

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllReviewsShould_RetrieveAllReviews_ThenReturnAsModelList()
    {
        // Arrange
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        await AddSampleData();

        // Act
        var reviewList = await _reviewService.GetAllReviews();

        // Assert
        Assert.NotNull(reviewList);
        Assert.True(reviewList.Any());
        Assert.True(reviewList.Count() == 2);
    }

    [Fact]
    public async Task GetReviewsByPropertyShould_FindAssociatedReviews_ThenReturnList()
    {
        // Arrange
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        await AddSampleData();

        // Act
        var reviewList = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == "A1");

        // Assert
        Assert.NotNull(reviewList);
        Assert.True(reviewList.Any());
        Assert.True(reviewList.Count() == 2);
    }

    [Fact]
    public async Task DeleteReviewByIdShould_DeleteReview_ThenReturnTrue()
    {
        // Arrange
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        var id = await AddSampleData();

        // Act
        var result = await _reviewService.DeleteReviewById(id);
        var reviewList = await _reviewService.GetAllReviews();

        // Assert
        Assert.True(result);
        Assert.True(reviewList.Count() == 1);
    }
}
