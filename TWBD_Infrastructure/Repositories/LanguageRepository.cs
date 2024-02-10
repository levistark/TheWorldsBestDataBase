﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
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

    // Create
    public override async Task<LanguageEntity> CreateAsync(LanguageEntity entity)
    {
        try
        {
            var result = await _productDataContext.Languages.AddAsync(entity);
            await _productDataContext.SaveChangesAsync();

            if (result.Entity == entity)
            {
                return result.Entity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.StackTrace); }
        return null!;
    }

    public override async Task<IEnumerable<LanguageEntity>> ReadAllAsync()
    {
        try
        {
            return await _productDataContext.Languages
                .Include(x => x.ProductDescriptions)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<LanguageEntity> ReadOneAsync(Expression<Func<LanguageEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _productDataContext.Languages
                .Include(x => x.ProductDescriptions)
                .FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
