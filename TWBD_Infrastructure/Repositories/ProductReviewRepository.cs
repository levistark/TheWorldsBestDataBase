using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class ProductReviewRepository : ProductRepo<ProductReviewEntity>
{
    private readonly ProductDataContext _productDataContext;
    public ProductReviewRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }
}
