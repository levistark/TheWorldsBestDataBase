using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class LanguageRepository : ProductRepo<LanguageEntity>
{
    private readonly ProductDataContext _productDataContext;
    public LanguageRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }
}
