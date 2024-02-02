using Microsoft.EntityFrameworkCore;
using TWBD_Domain.DTOs.Models;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
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
    public async Task<UserProfileModel> RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity()
    {
        // Arrange
        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

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
        var result = await _userService.RegisterUser(newUserRegistration);
        var createdUser = await _userService.GetUserProfileByEmail("levi@domain.com");
        UserProfileModel? profileModel = createdUser.ReturnObject as UserProfileModel;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(createdUser.ReturnObject);

        return profileModel!;
    }

    [Fact]
    public async void RegisterUserShould_RegisterBadNewUser_ThenReturnFalse()
    {
        // Arrange
        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);


        var newUserRegistration = new UserRegistrationModel()
        {
            FirstName = "Levi",
            LastName = "Stark",
            Email = "levi@domain.com",
            Password = "password",
            PasswordConfirm = "password",
            StreetName = "Hjälmhultsgatan 11",
            PostalCode = "25431",
        };

        // Act
        var result = await _userService.RegisterUser(newUserRegistration);
        UserModel? model = result.ReturnObject as UserModel;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.ReturnObject == null);
        Assert.True(result.Success == false);
    }

    [Fact]
    public async void GetUserProfileByIdShould_FindUserProfileWithGivenId_ThenReturnIt()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();
        
        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.GetUserProfileById(1);

        // Assert
        Assert.NotNull(result.ReturnObject);
        Assert.True(result.Success);
    }

    [Fact]
    public async void GetUserProfileByEmailShould_FindUserProfileWithGivenEmail_ThenReturnIt()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.GetUserProfileByEmail("levi@domain.com");

        // Assert
        Assert.NotNull(result.ReturnObject);
        Assert.True(result.Success);
    }

    [Fact]
    public async void GetAllUserProfilesShould_CollectUserProfiles_ThenReturnListOfUserProfileModels()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.GetAllUserProfiles();
        List<UserProfileModel>? userProfiles = result.ReturnObject as List<UserProfileModel>;

        // Assert
        Assert.NotNull(result.ReturnObject);
        Assert.True(result.Success);
        Assert.True(result.ReturnObject is List<UserProfileModel>);
        Assert.True(userProfiles!.Count() == 1);
    }

    [Fact]
    public async void UpdateUserProfileShould_FindAndUpdateCorrectUserProfile_ThenReturnTrue()
    {
        // Arrange
        var registeredUser = await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);
        var updatedUserProfile = new UserProfileModel()
        {
            UserId = 1,
            FirstName = "Hans",
            LastName = "Svag",
            Email = "asd@asd.com",
            City = "Sävsjö",
            StreetName = "Skogsrundan 31",
            PostalCode = "57633",
            Role = "Admin",
        };

        // Act
        var result = await _userService.UpdateUserProfile(updatedUserProfile);
        var updatedUser = await userRepository.ReadOneAsync(u => u.UserId == updatedUserProfile.UserId);

        // Assert
        Assert.True(result.Success);
        Assert.False(result.Code == null);
        Assert.True(result.Message == null);
        Assert.True(updatedUser.UserProfile!.FirstName == "Hans");
        Assert.True(updatedUser.UserProfile!.LastName == "Svag");
        Assert.True(updatedUser.UserAuthentication!.Email == "asd@asd.com");
        Assert.True(updatedUser.UserProfile!.Address.City == "Sävsjö");
        Assert.True(updatedUser.UserProfile!.Address.StreetName == "Skogsrundan 31");
        Assert.True(updatedUser.UserProfile!.Address.PostalCode == "57633");
    }

    [Fact]
    public async void DeleteUserByIdShould_FindAndDeleteUserWithGivenId_ThenReturnTrue()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.DeleteUserById(1);
        var dbCheck = await userRepository.ReadOneAsync(u => u.UserId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.Null(dbCheck);
    }

    [Fact]
    public async void DeleteUserByEmailShould_FindAndDeleteUserWithGivenEmail_ThenReturnTrue()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.DeleteUserByEmail("levi@domain.com");
        var dbCheck = await userRepository.ReadOneAsync(u => u.UserAuthentication!.Email == "levi@domain.com");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.Null(dbCheck);
    }

    [Fact]
    public async void DeleteUserProfileByIdShould_FindAndDeleteProfileWithGivenId_ThenReturnTrue()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.DeleteUserProfileById(1);
        var dbCheck = await userRepository.ReadOneAsync(u => u.UserProfile!.UserId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.Null(dbCheck);
    }

    [Fact]
    public async void DeleteUserProfileByEmailShould_FindAndDeleteProfileWithGivenEmail_ThenReturnTrue()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.DeleteUserProfileByEmail("levi@domain.com");
        var dbCheck = await userRepository.ReadOneAsync(u => u.UserAuthentication!.Email == "levi@domain.com");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.Null(dbCheck);
    }

    [Fact]
    public async void DeleteUserAuthenticationByIdShould_FindAndDeleteUserAuthenticationWithGivenId_ThenReturnTrue()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.DeleteUserAuthenticationById(1);
        var dbCheck = await userRepository.ReadOneAsync(u => u.UserAuthentication!.UserId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.Null(dbCheck);
    }

    [Fact]
    public async void DeleteUserAuthenticationByEmailShould_FindAndDeleteUserAuthenticationWithGivenEmail_ThenReturnTrue()
    {
        // Arrange
        await RegisterUserShould_RegisterNewUserToDb_ThenReturnWithUserEntity();

        UserAddressService _addressService = new UserAddressService(addressRepository);
        UserRoleService _userRoleService = new UserRoleService(roleRepository);
        UserSecurityService _userSecurityService = new UserSecurityService(authenticationRepository);
        UserValidationService _userValidationService = new UserValidationService(authenticationRepository);
        UserService _userService = new UserService(_userValidationService, userRepository, _addressService, authenticationRepository, profileRepository, _userRoleService, _userSecurityService);

        // Act
        var result = await _userService.DeleteUserAuthenticationByEmail("levi@domain.com");
        var dbCheck = await userRepository.ReadOneAsync(u => u.UserAuthentication!.Email == "levi@domain.com");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.True(result.Code == TWBD_Domain.DTOs.Enums.ServiceCode.DELETED);
        Assert.Null(dbCheck);
    }


}

