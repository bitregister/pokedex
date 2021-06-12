using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Serilog;
using TranslationEnum = Pokedex.Core.Enums.TranslationEnum;

namespace Pokedex.Core.Repositories
{
    public class FunTranslationsApiRepository : IFunTranslationsRepository
    {
        public async Task<Models.FunTranslations.Translation> GetTranslationAsync(string descriptionText, TranslationEnum translation)
        {
            try
            {
                if (translation == TranslationEnum.Yoda)
                {
                    var result = await "https://api.funtranslations.com"
                        .AppendPathSegment("translate")
                        .AppendPathSegment("yoda.json")
                        .AppendPathSegment(descriptionText)
                        .GetJsonAsync<Models.FunTranslations.Translation>();

                    return result;
                }

                Log.Error("Unsupported Translation");
                return null;
            }
            catch (FlurlHttpException e)
            {
                Log.Error(e, "Failed to translate text {Text}", descriptionText);
                throw;
            }
        }

        public Task<Models.FunTranslations.Translation> GetTranslationAsync(string descriptionText, Models.FunTranslations.Translation translation)
        {
            throw new System.NotImplementedException();
        }
    }
}
