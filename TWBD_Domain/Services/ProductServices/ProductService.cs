using System.Diagnostics;
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

    public ProductService(
        ProductRepository productRepository,
        LanguageRepository languageRepository,
        ManufacturerRepository manufacturerRepository,
        ProductCategoryRepository categoryRepository,
        ProductDescriptionRepository descriptionRepository,
        ProductReviewRepository reviewRepository)
    {
        _productRepository = productRepository;
        _languageRepository = languageRepository;
        _manufacturerRepository = manufacturerRepository;
        _categoryRepository = categoryRepository;
        _descriptionRepository = descriptionRepository;
        _reviewRepository = reviewRepository;
    }

    
}
