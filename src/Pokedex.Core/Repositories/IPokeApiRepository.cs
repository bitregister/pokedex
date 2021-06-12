using System.Threading.Tasks;
using Pokedex.Core.Models.PokeApi;

namespace Pokedex.Core.Repositories
{
    public interface IPokeApiRepository
    {
        public Task<Pokemon> GetPokemonByNameAsync(string name);
    }
}