using System.Threading.Tasks;
using Pokedex.Core.Enums;
using Pokedex.Core.Repositories;

namespace Pokedex.Core.Services.Translation
{
    public class ShakespeareTranslationStrategy : ITranslationStrategy
    {
        private readonly IFunTranslationsRepository _funTranslationsRepository;

        public ShakespeareTranslationStrategy(IFunTranslationsRepository funTranslationsRepository)
        {
            _funTranslationsRepository = funTranslationsRepository;
        }

        public async Task<string> Translate(string description)
        {
            try
            {
                var result = await _funTranslationsRepository.GetTranslationAsync(description, TranslationEnum.Shakespeare);
                return result.Contents.Translated;
            }
            catch
            {
                return description;
            }
        }
    }
}
