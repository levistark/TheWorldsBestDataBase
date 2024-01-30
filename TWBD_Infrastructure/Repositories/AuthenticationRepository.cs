using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;

namespace TWBD_Infrastructure.Repositories;
public class AuthenticationRepository : Repo<UserAuthenticationEntity>
{
    private readonly UserDataContext _userDataContext;
    public AuthenticationRepository(UserDataContext userDataContext) : base(userDataContext)
    {
        _userDataContext = userDataContext;
    }
}
