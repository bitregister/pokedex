using System.Threading.Tasks;
using Pokedex.Core.Models;

namespace Pokedex.Core.Repositories
{
    public interface IPokeApiRepository
    {
        public Task<Pokemon> GetPokemonByNameAsync(string name);
    }
}