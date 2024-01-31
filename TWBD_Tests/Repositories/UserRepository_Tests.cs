using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories;
public class UserRepository_Tests
{
    private readonly UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public async Task CreateUserShould_CreateNewUser_ThenReturnIt()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });

        var userEntity = new UserEntity()
        {
            IsActive = true,
            RoleId = 1
        };

        // Act
        var result = await _userRepository.CreateAsync(userEntity);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.True(result.UserId == 1);
    }

    [Fact]
    public async Task ReadOneUserShould_FindUser_ThenReturnIt()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });

        // Act
        var result = await _userRepository.ReadOneAsync(x => x.UserId == 2);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive == false);
        Assert.True(result.UserId == 2);
    }

    [Fact]
    public async Task ReadAllUsersShould_RetrieveAllUsers_ThenReturnList()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });

        // Act
        var result = await _userRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() == 2);
    }

    [Fact]
    public async Task UpdateUserShould_FindAndUpdateTheUserWithGivenId_ThenReturnIt()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });

        var newUser = new UserEntity() { UserId = 2, IsActive = true, RoleId = 2 };

        // Act
        var result = await _userRepository.UpdateAsync(x => x.UserId == 2, newUser);
        var userList = await _userRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive == true);
        Assert.True(userList.Count() == 2);
    }

    [Fact]
    public async Task DeleteUserShould_FindAndDeleteCorrectUser_ThenReturnTrue()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        var entityToDelete = await _userRepository.ReadOneAsync(x => x.UserId == 1);

        // Act
        var result = await _userRepository.DeleteAsync(x => x.UserId == 2, entityToDelete);
        var userList = await _userRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(userList.Count() == 1);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });

        // Act
        var entity = await _userRepository.Existing(a => a.UserId == 1);

        // Assert
        Assert.True(entity);
    }
}
