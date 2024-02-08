using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
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

    public override async Task<IEnumerable<ManufacturerEntity>> ReadAllAsync()
    {
        try
        {
            return await _productDataContext.Manufacturers
                .Include(x => x.Products).ThenInclude(x=> x.ProductReviews)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<ManufacturerEntity> ReadOneAsync(Expression<Func<ManufacturerEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _productDataContext.Manufacturers
                .Include(x => x.Products).ThenInclude(x => x.ProductReviews)
                .FirstOrDefaultAsync(predicate)!;

            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
