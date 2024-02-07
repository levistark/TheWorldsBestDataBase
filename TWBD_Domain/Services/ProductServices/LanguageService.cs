using System.Diagnostics;
using TWBD_Infrastructure.Entities;
using TWBD_Infrastructure.Repositories;

namespace TWBD_Domain.Services.ProductServices;
public class LanguageService
{

    private readonly LanguageRepository _languageRepository;

    public LanguageService(LanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    /// <summary>
    /// Finds an existing language id, or creates a new language if it does not exists
    /// </summary>
    /// <param name="language">The name of the related language</param>
    /// <returns>The category's id</returns>
    public async Task<int> GetLanguageId(string language)
    {
        try
        {
            var languageId = await _languageRepository.ReadOneAsync(l => l.Language == language);

            // Return existing category id
            if (languageId != null)
                return languageId.Id;

            // Create new category if it does not exists
            else
            {
                var newLanguage = await _languageRepository.CreateAsync(new LanguageEntity() { Language = language });
                if (newLanguage != null) return newLanguage.Id;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return 0;
    }

    public async Task<string> GetLanguageName(int id)
    {
        try
        {
            var language = await _languageRepository.ReadOneAsync(c => c.Id == id);

            if (language != null)
                return language.Language;
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
