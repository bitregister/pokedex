using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Pokedex.Core.Models;
using Serilog;

namespace Pokedex.Core.Repositories
{
    public class PokeApiRepository
    {
        public async Task<Pokemon> GetPokemonByName(string name)
        {
            try
            {
                var result = await "https://pokeapi.co"  //TODO: Error handling and logging, and injection of config
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