using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class ManufacturerRepository : ProductRepo<ManufacturerEntity>
{
    private readonly ProductDataContext _productDataContext;
    public ManufacturerRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }
}
