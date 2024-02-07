using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class ProductRepository : ProductRepo<ProductEntity>
{
    private readonly ProductDataContext _productDataContext;
    public ProductRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }
}
