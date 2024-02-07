using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class RoleRepository : UserRepo<UserRoleEntity>
{
    private readonly UserDataContext _userDataContext;
    public RoleRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
    public override async Task<IEnumerable<UserRoleEntity>> ReadAllAsync()
    {
        try
        {
            return await _userDataContext.Roles.Include(i => i.Users).ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<UserRoleEntity> ReadOneAsync(Expression<Func<UserRoleEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _userDataContext.Roles.Include(i => i.Users).FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
