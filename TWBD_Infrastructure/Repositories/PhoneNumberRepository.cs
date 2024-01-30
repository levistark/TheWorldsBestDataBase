using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class PhoneNumberRepository : Repo<UserRoleEntity>
{
    private readonly UserDataContext _userDataContext;
    public PhoneNumberRepository (UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
}
