using System.Diagnostics;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.DTOs.Responses;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly LanguageRepository _languageRepository;
    private readonly ManufacturerRepository _manufacturerRepository;
    private readonly ProductCategoryRepository _categoryRepository;
    private readonly ProductDescriptionRepository _descriptionRepository;
    private readonly ProductReviewRepository _reviewRepository;
    private readonly ManufacturerService _manufacturerService;
    private readonly CategoryService _categoryService;
    private readonly LanguageService _languageService;
    private readonly ReviewService _reviewService;
    private readonly DescriptionService _descriptionService;

    public ProductService(
        ProductRepository productRepository,
        LanguageRepository languageRepository,
        ManufacturerRepository manufacturerRepository,
        ProductCategoryRepository categoryRepository,
        ProductDescriptionRepository descriptionRepository,
        ProductReviewRepository reviewRepository,
        ManufacturerService manufacturerService,
        CategoryService categoryService,
        LanguageService languageService,
        ReviewService reviewService,
        DescriptionService descriptionService)
    {
        _productRepository = productRepository;
        _languageRepository = languageRepository;
        _manufacturerRepository = manufacturerRepository;
        _categoryRepository = categoryRepository;
        _descriptionRepository = descriptionRepository;
        _reviewRepository = reviewRepository;
        _manufacturerService = manufacturerService;
        _categoryService = categoryService;
        _languageService = languageService;
        _reviewService = reviewService;
        _descriptionService = descriptionService;
    }

    // Add product
    public async Task<ProductModel> CreateProduct(ProductRegistrationModel product)
    {
        try
        {
            var productEntity = await _productRepository.CreateAsync(new ProductEntity()
            {
                ArticleNumber = product.ArticleNumber,
                Title = product.Title,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ManufacturerId = await _manufacturerService.GetManufacturerId(product.Manufacturer),
                ProductCategoryId = await _categoryService.GetCategoryId(product.Category),
            });

            if (productEntity != null)
            {
                var descriptionEntity = await _descriptionRepository.CreateAsync(new ProductDescriptionEntity()
                {
                    Description = product.Description,
                    Specifications = product.Description,
                    ArticleNumber = product.ArticleNumber,
                    LanguageId = await _languageService.GetLanguageId(product.DescriptionLanguage)
                });

                if (descriptionEntity != null)
                {
                    return new ProductModel()
                    {
                        ArticleNumber = productEntity.ArticleNumber,
                        Title = productEntity.Title,
                        Manufacturer = productEntity.Manufacturer.Manufacturer,
                        Price = productEntity.Price,
                        DiscountPrice = productEntity.DiscountPrice,
                        Category = await _categoryService.CreateCategory(new CategoryRegistrationModel(product.Category, product.ParentCategory!)),
                        Descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == productEntity.ArticleNumber),
                        Reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == productEntity.ArticleNumber),
                    };
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Get product 
    public async Task<ProductModel> GetProductById(string articleNumber)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(articleNumber))
            {
                var entity = await _productRepository.ReadOneAsync(x => x.ArticleNumber == articleNumber);

                if (entity != null)
                {
                    return new ProductModel()
                    {
                        ArticleNumber = entity.ArticleNumber,
                        Title = entity.Title,
                        Manufacturer = entity.Manufacturer.Manufacturer,
                        Price = entity.Price,
                        DiscountPrice = entity.DiscountPrice,
                        Descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == entity.ArticleNumber),
                        Reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == entity.ArticleNumber),
                        Category = new CategoryModel()
                        {
                            Id = entity.ProductCategoryId,
                            Category = entity.ProductCategory.Category,
                            ParentCategory = await _categoryService.GetCategoryName(entity.ProductCategory.ParentCategory)
                        }
                    };
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Get all products
    public async Task<IEnumerable<ProductModel>> GetAllProducts()
    {
        try
        {
            List<ProductModel> productList = [];
            var enttityList = await _productRepository.ReadAllAsync();

            foreach (var entity in enttityList)
            {
                productList.Add(new ProductModel()
                {
                    ArticleNumber = entity.ArticleNumber,
                    Title = entity.Title,
                    Manufacturer = entity.Manufacturer.Manufacturer,
                    Price = entity.Price,
                    DiscountPrice = entity.DiscountPrice,
                    Descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == entity.ArticleNumber),
                    Reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == entity.ArticleNumber),
                    Category = new CategoryModel()
                    {
                        Id = entity.ProductCategoryId,
                        Category = entity.ProductCategory.Category,
                        ParentCategory = await _categoryService.GetCategoryName(entity.ProductCategory.ParentCategory)
                    }
                });
            }
            return productList;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Update product
    public async Task<ProductModel> UpdateProduct(ProductModel productUpdates)
    {
        try
        {
            var entityToUpdate = await _productRepository.ReadOneAsync(x => x.ArticleNumber == productUpdates.ArticleNumber);

            if (entityToUpdate != null)
            {
                var productUpdateResult = await _productRepository.UpdateAsync(x => x.ArticleNumber == productUpdates.ArticleNumber, new ProductEntity()
                {
                    ArticleNumber = entityToUpdate.ArticleNumber,
                    Title = productUpdates.Title,
                    ManufacturerId = await _manufacturerService.GetManufacturerId(productUpdates.Category.Category),
                    ProductCategoryId = await _categoryService.GetCategoryId(productUpdates.Category.Category)
                });

                if (productUpdateResult != null && productUpdateResult is ProductEntity)
                {
                    return new ProductModel()
                    {
                        ArticleNumber = productUpdateResult.ArticleNumber,
                        Title = productUpdateResult.Title,
                        Manufacturer = await _manufacturerService.GetManufacturerById(productUpdateResult.ManufacturerId),
                        Descriptions = await _descriptionService.GetDescriptionsByProperty(x => x.ArticleNumber == productUpdateResult.ArticleNumber),
                        Reviews = await _reviewService.GetReviewsByProperty(x => x.ArticleNumber == productUpdateResult.ArticleNumber),
                        Price = productUpdateResult.Price,
                        DiscountPrice = productUpdateResult.DiscountPrice,
                        Category = new CategoryModel()
                        {
                            Id = productUpdateResult.ProductCategoryId,
                            Category = productUpdateResult.ProductCategory.Category,
                            ParentCategory = await _categoryService.GetCategoryName(productUpdateResult.ProductCategory.ParentCategory)
                        }
                    };
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Delete product
    public async Task<bool> DeleteProductById(string articleNumber)
    {
        try
        {
            var productToDelete = await _productRepository.ReadOneAsync(x => x.ArticleNumber == articleNumber);
            
            if (productToDelete != null)
            {
                return await _productRepository.DeleteAsync(x => x.ArticleNumber == articleNumber, productToDelete);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

}
