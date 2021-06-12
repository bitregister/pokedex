using System.Threading.Tasks;
using Pokedex.Core.Enums;
using Pokedex.Core.Models.FunTranslations;

namespace Pokedex.Core.Repositories
{
    public interface IFunTranslationsRepository
    {
        Task<Translation> GetTranslationAsync(string descriptionText, TranslationEnum translation);
    }
}