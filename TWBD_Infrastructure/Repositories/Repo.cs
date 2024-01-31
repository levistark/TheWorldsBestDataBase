using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;

namespace TWBD_Infrastructure.Repositories;
public abstract class Repo<TEntity> where TEntity : class
{
    private readonly UserDataContext _userDataContext;

    protected Repo(UserDataContext userDataContext)
    {
        _userDataContext = userDataContext;
    }

    // Create
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            var result = await _userDataContext.Set<TEntity>().AddAsync(entity);
            await _userDataContext.SaveChangesAsync();

            if (result.Entity == entity)
            {
                return result.Entity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Read 
    public virtual async Task<TEntity> ReadOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _userDataContext.Set<TEntity>().FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> ReadAllAsync()
    {
        try
        {
            return await _userDataContext.Set<TEntity>().ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    // Update
    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity entity)
    {
        try
        {
            var existingEntity = await _userDataContext.Set<TEntity>().FirstOrDefaultAsync(expression);

            if (existingEntity != null)
            {
                _userDataContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _userDataContext.SaveChangesAsync();
                return existingEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!; 
    }

    // Delete
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression, TEntity entity)
    {
        try
        {
            var existingEntity = await _userDataContext.Set<TEntity>().FirstOrDefaultAsync(expression);

            if (existingEntity != null && existingEntity == entity)
            {
                _userDataContext.Set<TEntity>().Remove(entity); 
                await _userDataContext.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }

    public virtual async Task<bool> Existing(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            return await _userDataContext.Set<TEntity>().AnyAsync(expression);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
