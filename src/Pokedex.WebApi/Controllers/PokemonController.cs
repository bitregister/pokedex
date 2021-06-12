using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Flurl.Http;
using Pokedex.Core.Responses;
using Pokedex.Core.Services;

namespace Pokedex.WebApi.Controllers
{
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokeApiService _pokeApiService;

        public PokemonController(IPokeApiService pokeApiService)
        {
            _pokeApiService = pokeApiService;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PokemonResponse>> Index(string id)
        {
            try
            {
                var result = await _pokeApiService.GetPokemonByNameAsync(id);
                return result;
            }
            catch (FlurlHttpException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                    return NotFound();

                throw;
            }
            
        }

    }
}
