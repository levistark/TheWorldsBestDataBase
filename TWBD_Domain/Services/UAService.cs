using System.Diagnostics;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UAService(AuthenticationRepository uaRepository)
{
    private readonly AuthenticationRepository _uaRepository = uaRepository;

}
