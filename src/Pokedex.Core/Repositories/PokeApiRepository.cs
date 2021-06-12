using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Pokedex.Core.Models.PokeApi;
using Serilog;

namespace Pokedex.Core.Repositories
{
    public class PokeApiRepository : IPokeApiRepository
    {
        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            try
            {
                var result = await "https://pokeapi.co" 
                    .AppendPathSegment("api")
                    .AppendPathSegment("v2")
                    .AppendPathSegment("pokemon-species")
                    .AppendPathSegment(name)
                    .GetJsonAsync<Pokemon>();

                return result;
            }
            catch (FlurlHttpException e)
            {
                Log.Error(e, "Failed to find pokemon for id {Name}", name);
                throw;
            }
        }
    }
}