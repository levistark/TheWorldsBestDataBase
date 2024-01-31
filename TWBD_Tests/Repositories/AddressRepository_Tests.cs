using Microsoft.EntityFrameworkCore;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Repositories;
public class AddressRepository_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly RoleRepository _roleRepository = new(_userDataContext);
    private readonly UserRepository _userRepository = new(_userDataContext);
    private readonly AuthenticationRepository _uaRepository = new(_userDataContext);
    private readonly ProfileRepository _profileRepository = new(_userDataContext);
    private readonly AddressRepository _addressRepository = new(_userDataContext);

    [Fact]
    public async Task<IEnumerable<UserAddressEntity>> AddSampleDataShould_AddDataToTables_ReturnWithAddressList()
    {
        // Arrange
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Admin" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "User" });
        await _roleRepository.CreateAsync(new UserRoleEntity() { RoleType = "Manager" });

        var user1 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 1 });
        var user2 = await _userRepository.CreateAsync(new UserEntity() { IsActive = false, RoleId = 2 });
        var user3 = await _userRepository.CreateAsync(new UserEntity() { IsActive = true, RoleId = 3 });

        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user1.UserId, Email = "asd@asd.com", PasswordHash = "123", PasswordSalt = "123" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user2.UserId, Email = "dsa@dsa.com", PasswordHash = "321", PasswordSalt = "321" });
        await _uaRepository.CreateAsync(new UserAuthenticationEntity() { UserId = user3.UserId, Email = "singer@singer.com", PasswordHash = "111", PasswordSalt = "222" });

        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 1, FirstName = "Levi", LastName = "Stark" });
        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 2, FirstName = "Stefan", LastName = "Svensson" });
        await _profileRepository.CreateAsync(new UserProfileEntity() { UserId = 3, FirstName = "Richard", LastName = "Nikolausson" });

        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Helsingborg", StreetName = "Hjälmshultsgatan 11", PostalCode = "25431"});
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Sävsjö", StreetName = "Skogsrundan 31", PostalCode = "25431"});
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Sävsjö", StreetName = "Högaholmsgatan 3", PostalCode = "57632"});

        // Act
        var result = await _addressRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
        return result;
    }

    [Fact]
    public async Task CreateUserAddressShould_CreateNewUserAddress_ThenReturnIt()
    {
        // Arrange
        var address = new UserAddressEntity()
        {
            City = "Stockholm",
            StreetName = "Sveavägen 1",
            PostalCode = "12345",
        };

        // Act
        var result = await _addressRepository.CreateAsync(address);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StreetName == address.StreetName);
    }

    [Fact]
    public async Task ReadOneUserAddressShould_FindUserAddress_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();

        // Act
        var result = await _addressRepository.ReadOneAsync(x => x.AddressId == 1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.City == "Helsingborg");
    }

    [Fact]
    public async Task ReadAllUserAddressesShould_RetrieveAllAddresses_ThenReturnList()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();

        // Act
        var result = await _addressRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async Task UpdateUserAddressShould_FindAndUpdateTheUserAddressWithGivenId_ThenReturnIt()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var existingAddress = await _addressRepository.ReadOneAsync(x => x.AddressId == 1);

        // Act
        existingAddress.PostalCode = "25432";
        var result = await _addressRepository.UpdateAsync(x => x.AddressId == 1, existingAddress);
        var addressList = await _addressRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.City == "Helsingborg");
        Assert.True(addressList.Count() == 3);
    }

    [Fact]
    public async Task DeleteAddressShould_FindAndDeleteCorrectUserAddress_ThenReturnTrue()
    {
        // Arrange
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var entityToDelete = await _addressRepository.ReadOneAsync(a => a.AddressId == 1);

        // Act
        var result = await _addressRepository.DeleteAsync(x => x.PostalCode == "25431", entityToDelete);
        var addressList = await _addressRepository.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.True(!addressList.Any(b => b.City == "Helsingborg"));
        Assert.True(addressList.Count() == 2);
    }
}
