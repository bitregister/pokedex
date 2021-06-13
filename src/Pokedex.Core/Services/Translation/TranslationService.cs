using System;
using System.Threading.Tasks;
using Pokedex.Core.Enums;
using Pokedex.Core.Repositories;
using Pokedex.Core.Responses;
using Serilog;

namespace Pokedex.Core.Services.Translation
{
    public class TranslationService : ITranslationService
    {
        private readonly IFunTranslationsRepository _funTranslationsRepository;
        private readonly ITranslationStrategyFactory _translationStrategyFactory;

        public TranslationService(IFunTranslationsRepository funTranslationsRepository, ITranslationStrategyFactory translationStrategyFactory)
        {
            _funTranslationsRepository = funTranslationsRepository;
            _translationStrategyFactory = translationStrategyFactory;
        }

        public async Task<string> GetTranslation(PokemonResponse response)
        {
            try
            {
                string translation;
                if (response.IsLegendary || response.Habitat.Equals("Cave", StringComparison.InvariantCultureIgnoreCase))
                {
                    var translator = new Translator(_translationStrategyFactory.GetTranslationStrategy(TranslationEnum.Yoda, _funTranslationsRepository));
                    translation = await translator.TranslateAsync(response.Description);
                }
                else
                {
                    var translator = new Translator(_translationStrategyFactory.GetTranslationStrategy(TranslationEnum.Shakespeare, _funTranslationsRepository));
                    translation = await translator.TranslateAsync(response.Description);
                }

                return translation;
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to get translation.");
                return response.Description;
            }
        }
    }
}
