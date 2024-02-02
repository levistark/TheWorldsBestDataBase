using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs;
using TWBD_Domain.DTOs.Models;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services;
public class UserLoginService_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly UserRepository userRepository = new UserRepository(_userDataContext);
    private readonly AddressRepository addressRepository = new AddressRepository(_userDataContext);
    private readonly AuthenticationRepository authenticationRepository = new AuthenticationRepository(_userDataContext);
    private readonly ProfileRepository profileRepository = new ProfileRepository(_userDataContext);
    private readonly RoleRepository roleRepository = new RoleRepository(_userDataContext);

    [Fact]
    private async void LoginValidationShould_ValidateUserEmailAndPassword_ThenReturnTrue()
    {
        // Arrange
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserLoginService _userLoginService = new UserLoginService(userRepository, authenticationRepository, _userSecurityService);
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await userRepository.CreateAsync(new UserEntity() { RoleId = 1 });
        await userRepository.CreateAsync(new UserEntity() { RoleId = 2 });
        await authenticationRepository.CreateAsync(new UserAuthenticationEntity() { UserId = 1, Email = "asd@asd.com", PasswordHash = _userSecurityService.GenerateSecurePassword("password")});
        await authenticationRepository.CreateAsync(new UserAuthenticationEntity() { UserId = 2, Email = "dsa@dsa.com", PasswordHash = _userSecurityService.GenerateSecurePassword("password123") });

        var loginModel = new LoginModel("asd@asd.com", "password");

        // Act
        var result = await _userLoginService.LoginValidation(loginModel);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    private async void LoginValidationShould_ValidateUserEmailAndPassword_ThenReturnFalse()
    {
        // Arrange
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserLoginService _userLoginService = new UserLoginService(userRepository, authenticationRepository, _userSecurityService);
        RoleRepository _roleRepository = new RoleRepository(_userDataContext);
        UserRepository _userRepository = new UserRepository(_userDataContext);
        AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);

        await roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await userRepository.CreateAsync(new UserEntity() { RoleId = 1 });
        await userRepository.CreateAsync(new UserEntity() { RoleId = 2 });
        await authenticationRepository.CreateAsync(new UserAuthenticationEntity() { UserId = 1, Email = "asd@asd.com", PasswordHash = _userSecurityService.GenerateSecurePassword("password") });
        await authenticationRepository.CreateAsync(new UserAuthenticationEntity() { UserId = 2, Email = "dsa@dsa.com", PasswordHash = _userSecurityService.GenerateSecurePassword("password123") });

        var loginModel = new LoginModel("asd@asd.com", "password123");

        // Act
        var result = await _userLoginService.LoginValidation(loginModel);

        // Assert
        Assert.False(result.Success);
    }
}
