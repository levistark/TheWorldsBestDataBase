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
}
