using System.Diagnostics;
using TWBD_Domain.DTOs.Models;
using TWBD_Domain.Interfaces;
using TWBD_Infrastructure.Contexts;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services;
public class UserAddressService(AddressRepository addressRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;

    public async Task<int> GetAddressId(AddressModel address)
    {
        try
        {
            if (string.IsNullOrEmpty(address.City) || string.IsNullOrEmpty(address.StreetName) || string.IsNullOrEmpty(address.PostalCode))
            {
                return 0;
            }

            var addressEntity = new UserAddressEntity()
            {
                City = address.City,
                StreetName = address.StreetName,
                PostalCode = address.PostalCode,
            };

            var addressByStreet = await _addressRepository.ReadOneAsync(x => x.StreetName == addressEntity.StreetName);
            var addressByPostalCode = await _addressRepository.ReadOneAsync(x => x.PostalCode == addressEntity.PostalCode);

            if (addressByStreet == null || addressByPostalCode == null)
            {
                var createNewAddressResult = await _addressRepository.CreateAsync(addressEntity);
                return createNewAddressResult.AddressId;
            }

            if (addressByStreet!.AddressId == addressByPostalCode!.AddressId)
                return addressByStreet.AddressId;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return 0;
    }
}
