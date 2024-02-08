using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.Services.ProductServices;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services.Product;
public class ProductService_Tests
{
    private readonly static ProductDataContext _productDataContext =
        new(new DbContextOptionsBuilder<ProductDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ProductRepository _productRepository = new(_productDataContext);
    private readonly LanguageRepository _languageRepository = new(_productDataContext);
    private readonly ManufacturerRepository _manufacturerRepository = new(_productDataContext);
    private readonly ProductCategoryRepository _categoryRepository = new(_productDataContext);
    private readonly ProductDescriptionRepository _descriptionRepository = new(_productDataContext);
    private readonly ProductReviewRepository _reviewRepository = new(_productDataContext);

    [Fact]
    public async Task AddSampleData()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        LanguageService _languageService = new LanguageService(_languageRepository);
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        ProductService _productService = new ProductService(
            _productRepository,
            _languageRepository,
            _manufacturerRepository,
            _categoryRepository,
            _descriptionRepository,
            _reviewRepository,
            _manufacturerService,
            _categoryService,
            _languageService,
            _reviewService,
            _descriptionService);

        // Act

        var result = await _productService.CreateProduct(new ProductRegistrationModel()
        {
            ArticleNumber = "A1",
            Title = "iPhone",
            Manufacturer = "Apple",
            Description = "An iphone",
            DescriptionLanguage = "English",
            Price = 10000,
            DiscountPrice = 5000,
            Category = "iPhones",
            ParentCategory = "Cellphones",
        });

        var result2 = await _productService.CreateProduct(new ProductRegistrationModel()
        {
            ArticleNumber = "A2",
            Title = "iPhone 15 Pro",
            Manufacturer = "Apple",
            Description = "An iphone that's bit too expensive",
            DescriptionLanguage = "English",
            Price = 15000,
            DiscountPrice = 10000,
            Category = "iPhones",
            ParentCategory = "Cellphones",
        });

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateProductShould_AddNewProductWithRelationsToDb_ThenReturnWithModel()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        LanguageService _languageService = new LanguageService(_languageRepository);
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        ProductService _productService = new ProductService(
            _productRepository,
            _languageRepository,
            _manufacturerRepository,
            _categoryRepository,
            _descriptionRepository,
            _reviewRepository,
            _manufacturerService,
            _categoryService,
            _languageService,
            _reviewService,
            _descriptionService);

        // Act

        var result = await _productService.CreateProduct(new ProductRegistrationModel()
        {
            ArticleNumber = "A1",
            Title = "iPhone",
            Manufacturer = "Apple",
            Description = "An iphone",
            DescriptionLanguage = "English",
            Price = 10000,
            DiscountPrice = 5000,
            Category = "iPhones",
            ParentCategory = "Cellphones",
        });

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Title);
        Assert.NotNull(result.Manufacturer);
        Assert.NotNull(result.Category);
        Assert.NotNull(result.Descriptions);
        Assert.NotNull(result.Reviews);
        Assert.True(result.Price != 0);
        Assert.True(result.DiscountPrice != 0);
        Assert.True(result.ArticleNumber == "A1");
        Assert.True(result.Descriptions.Count() == 1);
        Assert.True(result.Reviews.Count() == 0);
    }

    [Fact]
    public async Task GetAllProductsShould_RetrieveAllProductsInDb_ReturnListOfModels()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        LanguageService _languageService = new LanguageService(_languageRepository);
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        ProductService _productService = new ProductService(
            _productRepository,
            _languageRepository,
            _manufacturerRepository,
            _categoryRepository,
            _descriptionRepository,
            _reviewRepository,
            _manufacturerService,
            _categoryService,
            _languageService,
            _reviewService,
            _descriptionService);

        await AddSampleData();

        // Act
        var result = await _productService.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 2);
    }

    [Fact]
    public async Task GetOneProductByIdShould_FindProduct_ThenReturnProductModel()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        LanguageService _languageService = new LanguageService(_languageRepository);
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        ProductService _productService = new ProductService(
            _productRepository,
            _languageRepository,
            _manufacturerRepository,
            _categoryRepository,
            _descriptionRepository,
            _reviewRepository,
            _manufacturerService,
            _categoryService,
            _languageService,
            _reviewService,
            _descriptionService);

        await AddSampleData();

        // Act
        var result = await _productService.GetProductById("A1");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ArticleNumber == "A1");
    }

    [Fact]
    public async Task UpdateProductShould_UpdateProductEntity_ThenReturnItAsModel()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        LanguageService _languageService = new LanguageService(_languageRepository);
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        ProductService _productService = new ProductService(
            _productRepository,
            _languageRepository,
            _manufacturerRepository,
            _categoryRepository,
            _descriptionRepository,
            _reviewRepository,
            _manufacturerService,
            _categoryService,
            _languageService,
            _reviewService,
            _descriptionService);

        await AddSampleData();

        // Act
        var result = await _productService.UpdateProduct(new ProductModel()
        {
            ArticleNumber = "A1",
            Title = "iPhone 11",
            Manufacturer = "Apple",
            Price = 10000,
            DiscountPrice = 5000,
            Category = new CategoryModel()
            {
                Id = 1,
                Category = "Cellphones",
                ParentCategory = "Phones"
            }
        });

        var productList = await _productService.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("iPhone 11", result.Title);
        Assert.True(productList.Count() == 2);
    }

    [Fact]
    public async Task DeleteProductByIdShould_DeleteProductAndItsRelations_ThenReturnTrue()
    {
        // Arrange
        ManufacturerService _manufacturerService = new ManufacturerService(_manufacturerRepository);
        CategoryService _categoryService = new CategoryService(_categoryRepository);
        LanguageService _languageService = new LanguageService(_languageRepository);
        ReviewService _reviewService = new ReviewService(_reviewRepository);
        DescriptionService _descriptionService = new DescriptionService(_descriptionRepository, _languageService);
        ProductService _productService = new ProductService(
            _productRepository,
            _languageRepository,
            _manufacturerRepository,
            _categoryRepository,
            _descriptionRepository,
            _reviewRepository,
            _manufacturerService,
            _categoryService,
            _languageService,
            _reviewService,
            _descriptionService);

        await AddSampleData();

        // Act
        var result = await _productService.DeleteProductById("A1");
        var productList = await _productService.GetAllProducts();
        var descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == "A1");
        var reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == "A1");
        var manufacturers = await _manufacturerRepository.ReadAllAsync();
        var categories = await _categoryRepository.ReadAllAsync();
        var languages = await _languageRepository.ReadAllAsync();

        Assert.True(result);
        Assert.True(productList.Count() == 1);
        Assert.True(descriptions.Count() == 0);
        Assert.True(reviews.Count() == 0);
        Assert.True(manufacturers.Count() == 1);
        Assert.True(languages.Count() == 1);
        Assert.True(categories.Count() > 1);
    }
}
