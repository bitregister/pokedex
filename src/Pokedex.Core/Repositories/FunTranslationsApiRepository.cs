using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Pokedex.Core.Enums;
using Pokedex.Core.Models.FunTranslations;
using Serilog;

namespace Pokedex.Core.Repositories
{
    public class FunTranslationsApiRepository : IFunTranslationsRepository
    {
        public async Task<Translation> GetTranslationAsync(string descriptionText, TranslationEnum translation)
        {
            try
            {
                switch (translation)
                {
                    case TranslationEnum.Yoda:
                    {
                        var result = await "https://api.funtranslations.com"
                            .AppendPathSegment("translate")
                            .AppendPathSegment("yoda.json")
                            .SetQueryParam("text", descriptionText)
                            .GetJsonAsync<Translation>();

                        return result;
                    }
                    case TranslationEnum.Shakespeare:
                    {
                        var result = await "https://api.funtranslations.com"
                            .AppendPathSegment("translate")
                            .AppendPathSegment("shakespeare.json")
                            .SetQueryParam("text", descriptionText)
                            .GetJsonAsync<Translation>();

                        return result;
                    }
                    default:
                        Log.Error("Unsupported Translation");
                        return null;
                }
            }
            catch (FlurlHttpException e)
            {
                Log.Error(e, "Failed to translate text {Text}", descriptionText);
                throw;
            }
        }
    }
}
