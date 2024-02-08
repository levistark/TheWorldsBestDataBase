using System.Diagnostics;
using System.Linq.Expressions;
using TWBD_Domain.DTOs.Models.Product;
using TWBD_Domain.DTOs.Responses;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class DescriptionService
{
    private readonly ProductDescriptionRepository _descriptionRepository;
    private readonly LanguageService _languageService;

    public DescriptionService(ProductDescriptionRepository descriptionRepository, LanguageService languageService)
    {
        _descriptionRepository = descriptionRepository;
        _languageService = languageService;
    }

    public async Task<DescriptionModel> CreateDescription(DescriptionModel description)
    {
        try
        {
            var result = await _descriptionRepository.CreateAsync(new ProductDescriptionEntity()
            {
                Description = description.Description,
                Specifications = description.Specifications,
                LanguageId = await _languageService.GetLanguageId(description.Language),
                Ingress = description.Ingress,
                ArticleNumber = description.ArticleNumber,
            });

            if (result != null)
            {
                return new DescriptionModel()
                {
                    Description = description.Description,
                    Specifications = description.Specifications,
                    Language = description.Language,
                    Ingress = description.Ingress,
                    ArticleNumber = description.ArticleNumber,
                };
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public async Task<List<DescriptionModel>> GetAllDescriptions()
    {
        try
        {
            List<DescriptionModel> descriptionList = [];
            var enttityList = await _descriptionRepository.ReadAllAsync();

            foreach (var entity in enttityList)
            {
                descriptionList.Add(new DescriptionModel()
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Specifications = entity.Specifications,
                    Language = await _languageService.GetLanguageName(entity.LanguageId),
                    Ingress = entity.Ingress,
                    ArticleNumber = entity.ArticleNumber,
                });;
            }

            return descriptionList;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    public async Task<List<DescriptionModel>> GetDescriptionsByProperty(Func<ProductDescriptionEntity, bool> predicate)
    {
        try
        {
            List<DescriptionModel> descriptionList = [];

            var entityList = await _descriptionRepository.ReadAllAsync();

            foreach (var entity in entityList)
            {
                if (predicate(entity))
                {
                    descriptionList.Add(new DescriptionModel()
                    {
                        Id = entity.Id,
                        Description = entity.Description,
                        Specifications = entity.Specifications,
                        Language = await _languageService.GetLanguageName(entity.LanguageId),
                        Ingress = entity.Ingress,
                        ArticleNumber = entity.ArticleNumber,
                    }); ;
                }
            }
            return descriptionList;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
    public async Task<DescriptionModel> UpdateDescription(DescriptionModel description)
    {
        try
        {
            var descriptionToUpdate = await _descriptionRepository.ReadOneAsync(description.ArticleNumber, await _languageService.GetLanguageId(description.Language));
            descriptionToUpdate.Description = description.Description;
            descriptionToUpdate.Specifications = description.Specifications;
            descriptionToUpdate.Ingress = description.Ingress;

            if (descriptionToUpdate != null)
            {
                var descriptionUpdate = await _descriptionRepository.UpdateAsync(x => x.Id == descriptionToUpdate.Id, descriptionToUpdate);

                if (descriptionUpdate != null)
                {
                    return new DescriptionModel()
                    {
                        Description = descriptionUpdate.Description,
                        Specifications = descriptionUpdate.Specifications,
                        Ingress = descriptionUpdate.Ingress,
                        Language = await _languageService.GetLanguageName(descriptionUpdate.LanguageId),
                        ArticleNumber = descriptionUpdate.ArticleNumber,
                    };
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
    public async Task<bool> DeleteDescription(DescriptionModel description)
    {
        try
        {
            var descriptionToDelete = await _descriptionRepository.ReadOneAsync(description.ArticleNumber, await _languageService.GetLanguageId(description.Language));

            if (descriptionToDelete != null)
            {
                return await _descriptionRepository.DeleteAsync(descriptionToDelete.ArticleNumber, descriptionToDelete.LanguageId, descriptionToDelete);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}
