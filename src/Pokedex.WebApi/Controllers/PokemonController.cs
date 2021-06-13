using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var result = await _pokeApiService.GetPokemonByNameAsync(id);
                return new JsonResult(result);
            }
            catch (FlurlHttpException e)
            {
                if (e.StatusCode == (int) HttpStatusCode.NotFound)
                    return NotFound();

                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult("Failed to process your request");
            }
        }

        [HttpGet]
        [Route("translated/{id}")]
        public async Task<IActionResult> Translated(string id)
        {
            try
            {
                var result = await _pokeApiService.GetPokemonByNameAsync(id, true);
                return new JsonResult(result);
            }
            catch (FlurlHttpException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                    return NotFound();

                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult("Failed to process your request");
            }

        }
    }
}
