using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.WebApi.Controllers
{
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public void Index(string id)
        {
            throw new NotImplementedException();
        }

    }
}
