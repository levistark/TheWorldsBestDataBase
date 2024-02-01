using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services;
public class UserRoleService_Tests
{

    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly RoleRepository _roleRepository = new(_userDataContext);

    [Fact]
    public async Task AddSampleDataShould_AddDataToTables_ReturnWithAddressList()
    {
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin"});
        await _roleRepository.CreateAsync(new UserRoleEntity() {RoleType = "Manager" });
        await _roleRepository.CreateAsync(new UserRoleEntity() {RoleType = "User" });

        // Act
        var result = await _roleRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async void GetRoleIdShould_CreateRoleWithNewRoleType_ThenReturnId()
    {
        // Arrange
        UserRoleService userRoleService = new(_roleRepository);
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var roleType = "Free";

        // Act
        var result = await userRoleService.GetRoleId(roleType);

        // Assert
        Assert.True(result != 0);
        Assert.True(result == 4);
    }

    [Fact]
    public async void GetRoleIdShould_FindTheRoleIdAssociatedWithRoleType_ThenReturnIt()
    {
        // Arrange
        UserRoleService userRoleService = new(_roleRepository);
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var roleType = "Admin";

        // Act
        var result = await userRoleService.GetRoleId(roleType);

        // Assert
        Assert.True(result != 0);
        Assert.True(result == 1);
    }

    [Fact]
    public async void GetRoleTypeShould_FindRoleTypeBasedOnId_ThenReturnType()
    {
        // Arrange
        UserRoleService userRoleService = new(_roleRepository);
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        int id = 1;

        // Act
        var result = await userRoleService.GetRoleType(id);

        // Assert
        Assert.NotNull(result);
        Assert.True(result == "Admin");
    }

}
