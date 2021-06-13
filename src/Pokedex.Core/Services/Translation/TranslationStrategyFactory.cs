using System;
using Pokedex.Core.Enums;
using Pokedex.Core.Repositories;

namespace Pokedex.Core.Services.Translation
{
    public class TranslationStrategyFactory : ITranslationStrategyFactory
    {
        public ITranslationStrategy GetTranslationStrategy(TranslationEnum translationEnum, IFunTranslationsRepository funTranslationsRepository)
        {
            return translationEnum switch
            {
                TranslationEnum.Yoda => new YodaTranslationStrategy(funTranslationsRepository),
                TranslationEnum.Shakespeare => new ShakespeareTranslationStrategy(funTranslationsRepository),
                _ => throw new Exception("Unsupported translation")
            };
        }
    }
}