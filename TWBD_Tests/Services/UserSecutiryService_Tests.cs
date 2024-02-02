using Microsoft.EntityFrameworkCore;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services;
public class UserSecutiryService_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly UserRepository userRepository = new UserRepository(_userDataContext);
    private readonly AddressRepository addressRepository = new AddressRepository(_userDataContext);
    private readonly AuthenticationRepository _authenticationRepository = new AuthenticationRepository(_userDataContext);
    private readonly ProfileRepository profileRepository = new ProfileRepository(_userDataContext);
    private readonly RoleRepository roleRepository = new RoleRepository(_userDataContext);
    

    [Fact]
    public void GenerateSecirePasswordShould_EncryptGivenPassword_ThenReturnIt()
    {
        // Arrange
        UserSecurityService _userSecurityService = new UserSecurityService(_authenticationRepository);
        var password = "password";

        // Act
        var result = _userSecurityService.GenerateSecurePassword(password);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(password, result);  
    }

    [Fact]
    public async void VerifyPasswordShould_VerifyGivenPasswordAgainsEmail_ThenReturnTrue()
    {
        // Arrange
        UserSecurityService _userSecurityService = new UserSecurityService(_authenticationRepository);
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        var user = await _userRepository.CreateAsync(new UserEntity() { RoleId = 1 });
        var ua = await _authenticationRepository.CreateAsync(new UserAuthenticationEntity()
        {
            UserId = 1,
            Email = "email@email.com",
            PasswordHash = _userSecurityService.GenerateSecurePassword("password")
        });

        // Act
        var result = await _userSecurityService.VerifyPassword("password", "email@email.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void VerifyPasswordShould_VerifyPasswordAgainstBadEmail_ThenReturnFalse()
    {
        // Arrange
        UserSecurityService _userSecurityService = new UserSecurityService(_authenticationRepository);
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        var user = await _userRepository.CreateAsync(new UserEntity() { RoleId = 1 });
        var ua = await _authenticationRepository.CreateAsync(new UserAuthenticationEntity()
        {
            UserId = 1,
            Email = "email@email.com",
            PasswordHash = _userSecurityService.GenerateSecurePassword("password")
        });

        // Act
        var result = await _userSecurityService.VerifyPassword("password", "email@ads.com");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async void VerifyPasswordShould_VerifyBadPasswordAgainstEmail_ThenReturnFalse()
    {
        // Arrange
        UserSecurityService _userSecurityService = new UserSecurityService(_authenticationRepository);
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);

        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        var user = await _userRepository.CreateAsync(new UserEntity() { RoleId = 1 });
        var ua = await _authenticationRepository.CreateAsync(new UserAuthenticationEntity()
        {
            UserId = 1,
            Email = "email@email.com",
            PasswordHash = _userSecurityService.GenerateSecurePassword("password")
        });

        // Act
        var result = await _userSecurityService.VerifyPassword("passworda", "email@email.com");

        // Assert
        Assert.False(result);
    }

}
