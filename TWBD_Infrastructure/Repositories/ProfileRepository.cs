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
}