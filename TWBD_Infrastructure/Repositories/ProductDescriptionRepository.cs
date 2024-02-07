using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class ProductDescriptionRepository : ProductRepo<ProductDescriptionEntity>
{
    private readonly ProductDataContext _productDataContext;
    public ProductDescriptionRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }

    public async Task<ProductDescriptionEntity> ReadOneAsync(string articleNumber, int languageId)
    {
        try
        {
            var existingEntity = await _productDataContext.ProductDescriptions
                .Include(x => x.Language)
                .Include(p => p.ArticleNumberNavigation)
                .FirstOrDefaultAsync(pd => pd.ArticleNumber == articleNumber && pd.LanguageId == languageId);
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public async Task<ProductDescriptionEntity> UpdateAsync(string articleNumber, int languageId, ProductDescriptionEntity entityWithUpdates)
    {
        try
        {
            var existingEntity = await _productDataContext.ProductDescriptions.
                FirstOrDefaultAsync(pd => pd.ArticleNumber == articleNumber && pd.LanguageId == languageId);

            if (existingEntity != null)
            {
                _productDataContext.Entry(existingEntity).CurrentValues.SetValues(entityWithUpdates);
                await _productDataContext.SaveChangesAsync();
                return existingEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Delete
    public virtual async Task<bool> DeleteAsync(string articleNumber, int languageId, ProductDescriptionEntity entityToBeDeleted)
    {
        try
        {
            var existingEntity = await _productDataContext.ProductDescriptions.
                FirstOrDefaultAsync(pd => pd.ArticleNumber == articleNumber && pd.LanguageId == languageId);

            if (existingEntity != null && existingEntity == entityToBeDeleted)
            {
                _productDataContext.ProductDescriptions.Remove(entityToBeDeleted);
                await _productDataContext.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public virtual async Task<bool> Existing(string articleNumber, int languageId)
    {
        try
        {
            return await _productDataContext.ProductDescriptions.AnyAsync(pd => pd.ArticleNumber == articleNumber && pd.LanguageId == languageId);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}