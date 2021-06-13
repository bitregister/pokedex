using System.Threading.Tasks;

namespace Pokedex.Core.Services.Translation
{
    public class Translator
    {
        private readonly ITranslationStrategy _translationStrategy;

        public Translator(ITranslationStrategy translationStrategy)
        {
            _translationStrategy = translationStrategy;
        }

        public async Task<string> TranslateAsync(string description)
        {
            return await _translationStrategy.Translate(description);
        }
    }
}
