using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories.UserRepositories;
public class AuthenticationRepository_Tests
{
    private readonly UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public async Task CreateUserAuthenticationShould_CreateNewUserAuthentications_ThenReturnIt()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        var user = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });

        // Act
        var result = await _uaRepository.CreateAsync(new UserAuthenticationEntity()
        {
            UserId = user.UserId,
            Email = "asd@asd.com",
            PasswordHash = "123",
        });

        // Assert
        Assert.NotNull(result);
        Assert.True(result.UserId == user.UserId);
    }


    [Fact]
    public async Task ReadOneUserAuthenticationShould_FindUserAuthentication_ThenReturnIt()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321" });

        // Act
        var result = await _uaRepository.ReadOneAsync(x => x.UserId == user1.UserId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Email == "asd@asd.com");
        Assert.True(result.UserId == 1);
    }

    [Fact]
    public async Task ReadAllUserAuthenticationsShould_RetrieveAllAuthentications_ThenReturnList()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321" });

        // Act
        var result = await _uaRepository.ReadAllAsync();

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
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321" });

        var newUserAuthentications = new UserAuthenticationEntity() { UserId = user1.UserId, Email = "levi@domain.com", PasswordHash = "2" };

        // Act
        var result = await _uaRepository.UpdateAsync(x => x.UserId == 1, newUserAuthentications);
        var uaList = await _uaRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Email == "levi@domain.com");
        Assert.True(uaList.Count() == 2);
    }

    [Fact]
    public async Task DeleteUserAuthenticationsShould_FindAndDeleteCorrectUserAuthentications_ThenReturnTrue()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321" });

        var entityToDelete = await _uaRepository.ReadOneAsync(x => x.UserId == 1);

        // Act
        var result = await _uaRepository.DeleteAsync(x => x.UserId == 1, entityToDelete);
        var uaList = await _uaRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(uaList.Count() == 1);
    }

    [Fact]
    public async Task ExistingShould_CheckIfEntityExists_ThenReturnTrueIfItExists()
    {
        // Arrange
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321" });

        // Act
        var entity = await _uaRepository.Existing(a => a.Email == "asd@asd.com");

        // Assert
        Assert.True(entity);
    }
}
