using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class UserRepository : Repo<UserEntity>
{
    private readonly UserDataContext _userDataContext;
    public UserRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
    public override async Task<IEnumerable<UserEntity>> ReadAllAsync()
    {
        try
        {
            return await _userDataContext.Users
                .Include(i => i.UserAuthentication)
                .Include(i => i.Role)
                .Include(i => i.UserProfile)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<UserEntity> ReadOneAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _userDataContext.Users
                .Include(i => i.UserAuthentication)
                .Include(i => i.Role)
                .Include(i => i.UserProfile)
                .FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
