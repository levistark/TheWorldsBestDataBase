using System.Diagnostics;
using TWBD_Domain.DTOs.Models.Product;
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
            var manufacturerEntity = await _manufacturerRepository.ReadOneAsync(m => m.Manufacturer == manufacturer.ToLower());

            // Return existing manufacturer id
            if (manufacturerEntity != null)
                return manufacturerEntity.Id;

            // Create new manufacturer if it does not exists
            else
            {
                var newManufacturer = await _manufacturerRepository.CreateAsync(new ManufacturerEntity() { Manufacturer = manufacturer.ToLower() });
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
    public async Task<IEnumerable<ManufacturerModel>> GetAllManufacturers()
    {
        try
        {
            List<ManufacturerModel> manufacturerList = [];
            var entityList = await _manufacturerRepository.ReadAllAsync();

            foreach (var entity in entityList)
                manufacturerList.Add(new ManufacturerModel(entity.Id, entity.Manufacturer));

            return manufacturerList;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message);}
        return null!;
    }
    public async Task<ManufacturerModel> UpdateManufacturer(ManufacturerModel model)
    {
        try
        {
            var manufacturerToUpdate = await _manufacturerRepository.ReadOneAsync(x => x.Id == model.Id);
            manufacturerToUpdate.Manufacturer = model.Manufacturer; 

            if (manufacturerToUpdate != null)
            {
                var result = await _manufacturerRepository.UpdateAsync(x => x.Id == model.Id, manufacturerToUpdate);

                if (result != null)
                {
                    return new ManufacturerModel(result.Id, result.Manufacturer);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
    public async Task<bool> DeleteManufacturerById(int id)
    {
        try
        {
            var manufacturerToDelete = await _manufacturerRepository.ReadOneAsync(x => x.Id == id);

            if (manufacturerToDelete != null)
            {
                return await _manufacturerRepository.DeleteAsync(x => x.Id == id, manufacturerToDelete);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
