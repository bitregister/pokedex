using System.Threading.Tasks;
using Pokedex.Core.Responses;

namespace Pokedex.Core.Services
{
    public interface IPokeApiService
    {
        Task<PokemonResponse> GetPokemonByNameAsync(string name, bool translate = false);
    }
}