using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs.Responses;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services;
public class UserValidationService_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly RoleRepository _roleRepository = new RoleRepository(_userDataContext);
    private readonly UserRepository _userRepository = new UserRepository(_userDataContext);
    private readonly AuthenticationRepository _uaRepository = new AuthenticationRepository(_userDataContext);
    private readonly UserValidationService _service = new UserValidationService(new AuthenticationRepository(_userDataContext));

    private async void AddSampleData()
    {
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = 1, Email = "asd@asd.com", PasswordHash = "123", PasswordSalt = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = 2, Email = "dsa@dsa.com", PasswordHash = "321", PasswordSalt = "321" });
    }

    [Fact]
    public void ValidatePasswordShould_ValidatePassword_ThenReturnTrue()
    {
        // Arrange
        string password = "BytMig123!";

        // Act
        var result = _service.ValidatePassword(password);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void ValidatePasswordShould_ValidatePassword_ThenReturnFalse()
    {
        // Arrange
        string password = "";

        // Act
        var result = _service.ValidatePassword(password);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        //Assert.True(result.Code == AuthenticationCode.INVALID_PASSWORD);
    }

    [Fact]
    public async void ValidateEmailShould_ValidateEmail_ThenReturnTrue()
    {
        // Arrange
        AddSampleData();
        string email = "levi@domain.com";

        // Act
        var result = await _service.ValidateEmail(email);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async void ValidateEmailShould_ValidateExistingEmail_ThenReturnFalse()
    {
        // Arrange
        AddSampleData();
        string email = "asd@asd.com";

        // Act
        var result = await _service.ValidateEmail(email);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
    }

    [Fact]
    public async void ValidateEmailShould_ValidateBadEmail_ThenReturnFalse()
    {
        // Arrange
        string email = "aa@aa.a";

        // Act
        var result = await _service.ValidateEmail(email);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void ValidateUpdatePasswordShould_ValidateNewAndOldPassword_ThenReturnTrue()
    {
        // Arrange
        string newPassword = "Bytmig321!";
        string oldPassword = "Bytmig123!";

        // Act
        var result = _service.ValidateUpdatePassword(newPassword, oldPassword);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public void ValidateUpdatePasswordShould_ValidateBadNewPassword_ThenReturnTrue()
    {
        // Arrange
        string newPassword = "123!";
        string oldPassword = "Bytmig123!";

        // Act
        var result = _service.ValidateUpdatePassword(newPassword, oldPassword);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
    }

    [Fact]
    public void ValidateUpdatePasswordShould_ValidateIdenticalNewPassword_ThenReturnTrue()
    {
        // Arrange
        string newPassword = "Bytmig123!";
        string oldPassword = "Bytmig123!";

        // Act
        var result = _service.ValidateUpdatePassword(newPassword, oldPassword);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
    }
}
