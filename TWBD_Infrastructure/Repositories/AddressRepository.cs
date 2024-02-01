using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.Linq.Expressions;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class AddressRepository : Repo<UserAddressEntity>
{
    private readonly UserDataContext _userDataContext;
    public AddressRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }

    public override async Task<IEnumerable<UserAddressEntity>> ReadAllAsync()
    {
        try
        {
            return await _userDataContext.Addresses.Include(i => i.UserProfiles).ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public override async Task<UserAddressEntity> ReadOneAsync(Expression<Func<UserAddressEntity, bool>> predicate)
    {
        try
        {
            var existingEntity = await _userDataContext.Addresses.Include(i => i.UserProfiles).FirstOrDefaultAsync(predicate)!;
            if (existingEntity != null)
                return existingEntity;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

}
