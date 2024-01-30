using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class UserRepository : Repo<UserRoleEntity>
{
    private readonly UserDataContext _userDataContext;
    public UserRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
}
