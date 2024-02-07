using System.Diagnostics;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class ManufacturerService
{
    private readonly ManufacturerRepository _manufacturerRepository;
    public ManufacturerService(ManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }
    public async Task<int> GetManufacturerId(string manufacturer)
    {
        try
        {
            var manufacturerEntity = await _manufacturerRepository.ReadOneAsync(m => m.Manufacturer == manufacturer);

            // Return existing manufacturer id
            if (manufacturerEntity != null)
                return manufacturerEntity.Id;

            // Create new manufacturer if it does not exists
            else
            {
                var newManufacturer = await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = manufacturer });
                if (newManufacturer != null) return newManufacturer.Id;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return 0;
    }

    public async Task<string> GetManufacturerById(int id)
    {
        try
        {
            var manufacturer = await _manufacturerRepository.ReadOneAsync(m => m.Id == id);

            if (manufacturer != null)
                return manufacturer.Manufacturer;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
