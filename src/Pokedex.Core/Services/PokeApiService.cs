using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pokedex.Core.Models.PokeApi;
using Pokedex.Core.Repositories;
using Pokedex.Core.Responses;
using Pokedex.Core.Services.Translation;
using Serilog;

namespace Pokedex.Core.Services
{
    public class PokeApiService : IPokeApiService
    {
        private readonly IPokeApiRepository _pokeApiRepository;
        private readonly ITranslationService _translationService;
        
        public PokeApiService(IPokeApiRepository pokeApiRepository, ITranslationService translationService)
        {
            _pokeApiRepository = pokeApiRepository;
            _translationService = translationService;
        }

        public async Task<PokemonResponse> GetPokemonByNameAsync(string name, bool translate = false)
        {
            Log.Debug("Getting Pokemon by name: {Name}", name);

            var result = await _pokeApiRepository.GetPokemonByNameAsync(name);
            var response = MapApiPokemonResponseToPokemonResponse(result);

            if (!translate)
            {
                Log.Debug("Pokemon Found @{Response}", response);
                return response;
            }

            var translation = await GetTranslation(response);

            var responseWithTranslation = response with {Description = translation};

            Log.Debug("Pokemon Found (with Translation) @{Response}", responseWithTranslation);

            return responseWithTranslation;
        }

        private async Task<string> GetTranslation(PokemonResponse response)
        {
            Log.Debug("Getting Pokemon Description Translation for: {Description}", response.Description);
            return await _translationService.GetTranslation(response);
        }

        private static PokemonResponse MapApiPokemonResponseToPokemonResponse(Pokemon pokemon)
        {
            var description = GetDescription(pokemon.FlavorTextEntries);
            var habitatName = GetHabitatName(pokemon.Habitat);

            var pokemonResponse = new PokemonResponse(pokemon.Id, pokemon.Name, pokemon.IsLegendary, description, habitatName);
            return pokemonResponse;
        }

        private static string GetDescription(IEnumerable<FlavorTextEntry> flavorTextEntries)
        {
            var firstEnglishEntry = flavorTextEntries.FirstOrDefault(y => y.Language.Name.Equals("en"));

            if (firstEnglishEntry == null)
            {
                return string.Empty;
            }
            
            var result = SanitiseDescriptionText(firstEnglishEntry);

            return result;
        }

        private static string GetHabitatName(Habitat habitat)
        {
            return habitat != null ? habitat.Name : string.Empty;
        }

        private static string SanitiseDescriptionText(FlavorTextEntry firstEnglishEntry)
        {
            //Ensure that all carriage returns and line feeds are removed
            var result = Regex.Replace(firstEnglishEntry.Description, @"\r|\n|\f", " ");
            return result;
        }
    }
}
