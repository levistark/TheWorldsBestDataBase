using Microsoft.EntityFrameworkCore;
using System.Net;
using TWBD_Domain.DTOs;
using TWBD_Domain.Services;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Tests.Services;
public class UserAddressService_Tests
{
    private readonly static UserDataContext _userDataContext =
        new(new DbContextOptionsBuilder<UserDataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly AddressRepository _addressRepository = new(_userDataContext);

    [Fact]
    public async Task AddSampleDataShould_AddDataToTables_ReturnWithAddressList()
    {
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Helsingborg", StreetName = "Hjälmshultsgatan 11", PostalCode = "25431" });
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Sävsjö", StreetName = "Skogsrundan 31", PostalCode = "25431" });
        await _addressRepository.CreateAsync(new UserAddressEntity() { City = "Sävsjö", StreetName = "Högaholmsgatan 3", PostalCode = "57632" });

        // Act
        var result = await _addressRepository.ReadAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count() > 2);
    }

    [Fact]
    public async void GetAddressIdShould_CreateNewAddressIfNotExisting_ThenReturnNewAddressId()
    {
        // Arrange
        UserAddressService userAddressService = new(_addressRepository);
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var addressEntity = new AddressModel()
        {
            City = "Helsingborg",
            StreetName = "Hjälmshultsgatan 12",
            PostalCode = "25431",
        };

        // Act
        var result = await userAddressService.GetAddressId(addressEntity);

        // Assert
        Assert.True(result != 0);
        Assert.True(result == 4);
    }

    [Fact]
    public async void GetAddressIdShould_CreateNewAddressIfNotExisting_ThenReturnExistingAddressId()
    {
        // Arrange
        UserAddressService userAddressService = new(_addressRepository);
        await AddSampleDataShould_AddDataToTables_ReturnWithAddressList();
        var addressEntity = new AddressModel()
        {
            City = "Helsingborg",
            StreetName = "Hjälmshultsgatan 11",
            PostalCode = "25431",
        };

        // Act
        var result = await userAddressService.GetAddressId(addressEntity);

        // Assert
        Assert.True(result != 0);
        Assert.True(result == 1);
    }
}
