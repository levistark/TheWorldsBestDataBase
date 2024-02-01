using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services;
public class UserService_Tests
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
    public async void CreateUserShould_RegisterNewUserToDb_ThenReturnUserModel()
    {
        // Arrange
        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserRegisterService _userRegisterService = new UserRegisterService(
            userRepository,
            _addressService,
            authenticationRepository,
            profileRepository,
            _userRoleService,
            _userSecurityService);
        UserService _userService = new UserService(_userValidationService, _userRegisterService);

        var newUserRegistration = new UserRegistrationModel()
        {
            FirstName = "Levi",
            LastName = "Stark",
            Email = "levi@domain.com",
            Password = "BytMig123!",
            PasswordConfirm = "BytMig123!",
            City = "Helsingborg",
            StreetName = "Hjälmhultsgatan 11",
            PostalCode = "25431",
            Role = "Admin",
        };

        // Act
        var result = await _userService.CreateUser(newUserRegistration);
        UserModel? model = result.ReturnObject as UserModel;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ReturnObject is UserModel);
        Assert.True(model!.FirstName == "Levi");
    }
}
