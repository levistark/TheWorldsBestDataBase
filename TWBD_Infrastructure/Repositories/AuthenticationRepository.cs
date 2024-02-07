using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class AuthenticationRepository : UserRepo<UserAuthenticationEntity>
{
    private readonly UserDataContext _userDataContext;
    public AuthenticationRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
    public override async Task<IEnumerable<UserAuthenticationEntity>> ReadAllAsync()
    {
        try
        {
            return await _userDataContext.Authentications.Include(i => i.User).ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<UserAuthenticationEntity> ReadOneAsync(Expression<Func<UserAuthenticationEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _userDataContext.Authentications.Include(i => i.User).FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
