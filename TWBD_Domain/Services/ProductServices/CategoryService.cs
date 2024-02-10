using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.DTOs.Responses;
using TWBD_Domain.DTOs.Enums;
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
    /// Creates a new category from a ProductRegistrationModel class, adds it to the db, including parent categories
    /// </summary>
    /// <param name="product">ProductRegistrationModel class</param>
    /// <returns>CategoryModel</returns>
    public async Task<CategoryModel> CreateCategory(CategoryRegistrationModel category)
    {
        try
        {
            if (category != null)
            {
                var addedCategory = await _categoryRepository.CreateAsync(new ProductCategoryEntity()
                {
                    Category = category.Category,
                    ParentCategory = await GetCategoryId(category.ParentCategory!)
                });

                if (addedCategory != null)
                {
                    return new CategoryModel()
                    {
                        Id = addedCategory.Id,
                        Category = addedCategory.Category,
                        ParentCategory = await GetCategoryName(addedCategory.ParentCategory!)
                    };
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public async Task<ServiceResponse> UpdateCategory(CategoryModel model)
    {
        try
        {
            var categoryExists = await _categoryRepository.Existing(x => x.Id == model.Id);
            var existingCategory = await _categoryRepository.ReadOneAsync(x => x.Id == model.Id);

            if (categoryExists)
            {
                if (model.ParentCategory != null)
                {
                    var updatedEntity = await _categoryRepository.UpdateAsync(x => x.Id == model.Id, new ProductCategoryEntity()
                    {
                        Id = existingCategory.Id,
                        Category = model.Category,
                        ParentCategory = await GetCategoryId(model.ParentCategory)
                    });

                    if (updatedEntity != null)
                    {
                        return new ServiceResponse()
                        {
                            Success = true,
                            ReturnObject = new CategoryModel()
                            {
                                Id = existingCategory.Id,
                                Category = updatedEntity.Category,
                                ParentCategory = await GetCategoryName(updatedEntity.ParentCategory)
                            },
                            Code = ServiceCode.UPDATED
                        };
                    }
                }
                else
                {
                    var updatedEntity = await _categoryRepository.UpdateAsync(x => x.Id == model.Id, new ProductCategoryEntity()
                    {
                        Id = model.Id,
                        Category = model.Category,
                    });

                    if (updatedEntity != null)
                    {
                        return new ServiceResponse()
                        {
                            Success = true,
                            ReturnObject = new CategoryModel()
                            {
                                Id = existingCategory.Id,
                                Category = updatedEntity.Category,
                                ParentCategory = await GetCategoryName(updatedEntity.ParentCategory)
                            },
                            Code = ServiceCode.UPDATED
                        };
                    }
                }
            }
            else
            {
                return new ServiceResponse()
                {
                    Success = false,
                    Code = ServiceCode.NOT_FOUND
                };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse() { Code = ServiceCode.FAILED };
    }

    public async Task<ServiceResponse> DeleteCategoryById(int id)
    {
        try
        {
            var entityToDelete = await _categoryRepository.ReadOneAsync(x => x.Id == id);

            if (await _categoryRepository.Existing(x => x.Id == id))
            {
                var deletionResult = await _categoryRepository.DeleteAsync(x => x.Id == id, entityToDelete);

                if (deletionResult)
                {
                    return new ServiceResponse()
                    {
                        Success = true,
                        Code = ServiceCode.DELETED
                    };
                }
                else
                    return new ServiceResponse();
            }
            else
            {
                return new ServiceResponse()
                {
                    Success = false,
                    Code = ServiceCode.NOT_FOUND
                }; 
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return new ServiceResponse() { Code = ServiceCode.FAILED };
    }

    /// <summary>
    /// Finds an existing category id, or creates a new category if it does not exists
    /// </summary>
    /// <param name="category">The name of the related category</param>
    /// <returns>The category's id</returns>
    public async Task<int?> GetCategoryId(string category)
    {
        try
        {
            var categoryEntity = await _categoryRepository.ReadOneAsync(c => c.Category == category.ToLower());

            // Return existing category id
            if (categoryEntity != null)
                return categoryEntity.Id;

            // Create new category if it does not exists
            else
            {
                var newCategory = await _categoryRepository.CreateAsync(new ProductCategoryEntity() 
                { 
                    Category = category.ToLower(),
                });

                 if (newCategory != null) return newCategory.Id;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
    public async Task<int?> GetCategoryId(string category, int parentCategoryId)
    {
        try
        {
            var categoryEntity = await _categoryRepository.ReadOneAsync(c => c.Category == category.ToLower());

            // Return existing category id
            if (categoryEntity != null)
                return categoryEntity.Id;

            // Create new category if it does not exists
            else
            {
                var newCategory = await _categoryRepository.CreateAsync(new ProductCategoryEntity()
                {
                    Category = category.ToLower(),
                    ParentCategory = parentCategoryId 
                });

                if (newCategory != null) return newCategory.Id;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
    public async Task<string> GetCategoryName(int? id)
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

    public async Task<string> GetParentCategoryName(int id)
    {
        try
        {
            var category = await _categoryRepository.ReadOneAsync(c => c.Id == id);
            var parentCategory = await GetCategoryName(category.ParentCategory);

            if (parentCategory != null)
                return parentCategory;
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
