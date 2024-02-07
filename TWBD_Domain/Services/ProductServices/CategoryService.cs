using System.Diagnostics;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class CategoryService
{
    private readonly ProductCategoryRepository _categoryRepository;

    public CategoryService(ProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Finds an existing category id, or creates a new category if it does not exists
    /// </summary>
    /// <param name="category">The name of the related category</param>
    /// <returns>The category's id</returns>
    public async Task<int> GetCategoryId(string category)
    {
        try
        {
            var categoryId = await _categoryRepository.ReadOneAsync(c => c.Category == category);

            // Return existing category id
            if (categoryId != null)
                return categoryId.Id;

            // Create new category if it does not exists
            else
            {
                var newCategory = await _categoryRepository.CreateAsync(new ProductCategoryEntity() { Category = category });
                if (newCategory != null) return newCategory.Id;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return 0;
    }


    public async Task<string> GetCategoryName(int id)
    {
        try
        {
            var category = await _categoryRepository.ReadOneAsync(c => c.Id == id);

            if (category != null)
                return category.Category;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    /// <summary>
    /// Finds and returns the parent category id from a child categories id
    /// </summary>
    /// <param name="childId">Id of the category child</param>
    /// <returns></returns>
    public async Task<int> GetParentCategoryId(int childId)
    {
        try
        {
            var result = await _categoryRepository.ReadOneAsync(c => c.Id == childId);

            if (result != null)
                return (int)result.ParentCategory!;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return 0;
    }
}
