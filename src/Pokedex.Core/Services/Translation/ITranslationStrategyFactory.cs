using Pokedex.Core.Enums;
using Pokedex.Core.Repositories;

namespace Pokedex.Core.Services.Translation
{
    public interface ITranslationStrategyFactory
    {
        ITranslationStrategy GetTranslationStrategy(TranslationEnum translationEnum, IFunTranslationsRepository funTranslationsRepository);
    }
}