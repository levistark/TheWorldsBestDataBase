using System.Diagnostics;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class ProductValidationService
{
    private readonly ProductRepository _productRepository;

    public ProductValidationService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> ValidateArticleNumber(string articleNumber)
    {
        try
        { 
            if (!await _productRepository.Existing(x => x.ArticleNumber == articleNumber))
                return true;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
