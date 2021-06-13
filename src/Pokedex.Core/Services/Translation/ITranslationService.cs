using System.Threading.Tasks;
using Pokedex.Core.Responses;

namespace Pokedex.Core.Services.Translation
{
    public interface ITranslationService
    {
        Task<string> GetTranslation(PokemonResponse response);
    }
}