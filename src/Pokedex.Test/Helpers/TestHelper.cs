using System.Collections.Generic;
using Pokedex.Core.Models.PokeApi;

namespace Pokedex.Core.Test.Helpers
{
    public class TestHelper
    {
        public static Pokemon ConfigurePokemon(string id = "1", string name = "MewTwo", bool isLegendary = true, string description = "The Description", string languageName = "en", string habitatName = "Cave")
        {
            var language = new Language { Name = languageName };

            var flavorTextEntry = new FlavorTextEntry { Description = description, Language = language };
            var flavorTextExtraEntry = new FlavorTextEntry { Description = "another description", Language = language };
            var flavorTextSpanishEntry = new FlavorTextEntry { Description = "spanish description", Language = new Language(){ Name = "ES"} };

            var flavorTextEntryList = new List<FlavorTextEntry> { flavorTextEntry, flavorTextExtraEntry, flavorTextSpanishEntry };

            Habitat habitat = null;

            if (habitatName != null)
            {
                habitat = new Habitat { Name = habitatName };
            }

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
