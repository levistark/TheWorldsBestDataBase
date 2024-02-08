using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
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

    public override async Task<IEnumerable<ProductReviewEntity>> ReadAllAsync()
    {
        try
        {
            return await _productDataContext.ProductReviews
                .Include(x => x.ArticleNumberNavigation).ThenInclude(x => x.Manufacturer)
                .Include(x => x.ArticleNumberNavigation).ThenInclude(x => x.ProductDescriptions)!.ThenInclude(x => x.Language)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<ProductReviewEntity> ReadOneAsync(Expression<Func<ProductReviewEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _productDataContext.ProductReviews
                .Include(x => x.ArticleNumberNavigation).ThenInclude(x => x.Manufacturer)
                .Include(x => x.ArticleNumberNavigation).ThenInclude(x => x.ProductDescriptions)!.ThenInclude(x => x.Language).FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
