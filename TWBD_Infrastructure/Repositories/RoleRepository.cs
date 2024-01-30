using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class RoleRepository : Repo<UserRoleEntity>
{
    private readonly UserDataContext _userDataContext;
    public RoleRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
}
