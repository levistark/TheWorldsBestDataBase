using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class ProfileRepository : Repo<UserProfileEntity>
{
    private readonly UserDataContext _userDataContext;
    public ProfileRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }

    public override async Task<IEnumerable<UserProfileEntity>> ReadAllAsync()
    {
        try
        {
            return await _userDataContext.Profiles
                .Include(i => i.User)
                .ThenInclude(i => i.UserAuthentication)
                .Include(i => i.User)
                .ThenInclude(i => i.Role)
                .Include(i => i.Address)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<UserProfileEntity> ReadOneAsync(Expression<Func<UserProfileEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _userDataContext.Profiles
                .Include(i => i.User)
                .ThenInclude(i => i.UserAuthentication)
                .Include(i => i.User)
                .ThenInclude(i => i.Role)
                .Include(i => i.Address)
                .FirstOrDefaultAsync(predicate)!;

            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}