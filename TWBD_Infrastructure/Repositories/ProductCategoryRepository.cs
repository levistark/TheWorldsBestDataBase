using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
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

    public override async Task<IEnumerable<ProductCategoryEntity>> ReadAllAsync()
    {
        try
        {
            return await _productDataContext.ProductCategories
                .Include(x => x.Products)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<ProductCategoryEntity> ReadOneAsync(Expression<Func<ProductCategoryEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _productDataContext.ProductCategories
                .Include(x => x.Products)
                .FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
