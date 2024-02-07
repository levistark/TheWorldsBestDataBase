using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class ProductCategoryRepository : ProductRepo<ProductCategoryEntity>
{
    private readonly ProductDataContext _productDataContext;
    public ProductCategoryRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }
}
