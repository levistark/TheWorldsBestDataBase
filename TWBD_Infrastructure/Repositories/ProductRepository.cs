using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace TWBD_Infrastructure.Repositories;
public class ProductRepository : ProductRepo<ProductEntity>
{
    private readonly ProductDataContext _productDataContext;
    public ProductRepository(ProductDataContext productDataContext) : base(productDataContext)
    {
        _productDataContext = productDataContext;
    }

    public override async Task<IEnumerable<ProductEntity>> ReadAllAsync()
    {
        try
        {
            return await _productDataContext.Products
                .Include(x => x.Manufacturer)
                .Include(x => x.ProductReviews)
                .Include(x => x.ProductDescriptions)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<ProductEntity> ReadOneAsync(Expression<Func<ProductEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _productDataContext.Products
                .Include(x => x.Manufacturer)
                .Include(x => x.ProductDescriptions)!.ThenInclude(x => x.Language)
                .Include(x => x.ProductReviews)
                .FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<bool> DeleteAsync(Expression<Func<ProductEntity, bool>> predicate, ProductEntity entity)
    {
        try
        {
            var existingEntity = await _productDataContext.Products.FirstOrDefaultAsync(predicate);

            if (existingEntity != null && existingEntity == entity)
            {

                //foreach (var description in existingEntity.ProductDescriptions!.ToList())
                //{
                //    _productDataContext.ProductDescriptions.Remove(description);
                //}
                //
                //foreach (var review in existingEntity.ProductReviews!.ToList())
                //{
                //    _productDataContext.ProductReviews.Remove(review);
                //}

                _productDataContext.Products.Remove(existingEntity!);
                await _productDataContext.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message);}
        return false;
        
    }
}
