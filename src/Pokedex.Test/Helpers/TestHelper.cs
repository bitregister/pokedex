using System.Collections.Generic;
using Pokedex.Core.Models;

namespace Pokedex.Core.Test.Helpers
{
    public class TestHelper
    {
        public static Pokemon ConfigurePokemon(string id = "1", string name = "MewTwo", bool isLegendary = true, string description = "The Description", string languageName = "en")
        {
            var language = new Language { Name = languageName };

            var flavorTextEntry = new FlavorTextEntry { Description = description, Language = language };
            var flavorTextEntryList = new List<FlavorTextEntry> { flavorTextEntry };

            var habitat = new Habitat {Name = "Cave"};

            var pokemon = new Pokemon
            {
                Id = id,
                IsLegendary = isLegendary,
                Name = name,
                FlavorTextEntries = flavorTextEntryList,
                Habitat = habitat
            };

            return pokemon;
        }
    }
}
