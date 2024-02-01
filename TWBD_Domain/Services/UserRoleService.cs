using System.Diagnostics;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UserRoleService
{
    private readonly RoleRepository _roleRepository;

    public UserRoleService(RoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<int> GetRoleId(string roleType)
    {
        try
        {
            if (roleType != "" || roleType == null)
            {
                var existingRoleType = await _roleRepository.ReadOneAsync(x => x.RoleType == roleType);

                if (existingRoleType != null)
                    return existingRoleType.RoleId;
                else
                {
                    var result = await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = roleType! });
                    return result.RoleId;
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return 0;
    }

    public async Task<string> GetRoleType(int roleId)
    {
        try
        {
            var roleType = await _roleRepository.ReadOneAsync(x => x.RoleId == roleId);
            return roleType.RoleType;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
