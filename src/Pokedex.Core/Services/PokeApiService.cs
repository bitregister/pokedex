﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pokedex.Core.Models;
using Pokedex.Core.Repositories;
using Pokedex.Core.Responses;

namespace Pokedex.Core.Services
{
    public class PokeApiService
    {
        private readonly IPokeApiRepository _pokeApiRepository;

        public PokeApiService(IPokeApiRepository pokeApiRepository)
        {
            _pokeApiRepository = pokeApiRepository;
        }

        public async Task<PokemonResponse> GetPokemonByNameAsync(string name)
        {
            var result = await _pokeApiRepository.GetPokemonByNameAsync(name);
            var response = MapApiPokemonResponseToPokemonResponse(result);
            
            return response;
        }

        private static PokemonResponse MapApiPokemonResponseToPokemonResponse(Pokemon pokemon)
        {
            var description = GetDescription(pokemon.FlavorTextEntries);

            var pokemonResponse = new PokemonResponse(pokemon.Id, pokemon.Name, pokemon.IsLegendary, description);
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

        private static string SanitiseDescriptionText(FlavorTextEntry firstEnglishEntry)
        {
            //Ensure that all carriage returns and line feeds are removed
            var result = Regex.Replace(firstEnglishEntry.Description, @"\r|\n|\f", " ");
            return result;
        }
    }
}
