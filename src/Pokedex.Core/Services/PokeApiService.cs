using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pokedex.Core.Models.PokeApi;
using Pokedex.Core.Repositories;
using Pokedex.Core.Responses;
using Pokedex.Core.Services.Translation;

namespace Pokedex.Core.Services
{
    public class PokeApiService : IPokeApiService
    {
        private readonly IPokeApiRepository _pokeApiRepository;
        private readonly IFunTranslationsRepository _funTranslationsRepository;

        public PokeApiService(IPokeApiRepository pokeApiRepository, IFunTranslationsRepository funTranslationsRepository)
        {
            _pokeApiRepository = pokeApiRepository;
            _funTranslationsRepository = funTranslationsRepository;
        }

        public async Task<PokemonResponse> GetPokemonByNameAsync(string name, bool translate = false)
        {
            var result = await _pokeApiRepository.GetPokemonByNameAsync(name);
            var response = MapApiPokemonResponseToPokemonResponse(result);

            if (!translate)
            {
                return response;
            }

            var translation = await GetTranslation(response);

            var responseWithTranslation = response with {Description = translation};
            return responseWithTranslation;
        }

        private async Task<string> GetTranslation(PokemonResponse response)
        {
            string translation;

            if (response.IsLegendary || response.Habitat.Equals("Cave", StringComparison.InvariantCultureIgnoreCase))
            {
                var translator = new Translator(new YodaTranslationStrategy(_funTranslationsRepository));
                translation = await translator.TranslateAsync(response.Description);
            }
            else
            {
                var translator = new Translator(new ShakespeareTranslationStrategy(_funTranslationsRepository));
                translation = await translator.TranslateAsync(response.Description);
            }

            return translation;
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
