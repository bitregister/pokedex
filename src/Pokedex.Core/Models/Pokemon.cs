using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pokedex.Core.Models
{
    public record Pokemon()
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("Is_Legendary")]
        public bool IsLegendary { get; set; }

        [JsonProperty("Flavor_Text_Entries")]
        public IEnumerable<FlavorTextEntry> FlavorTextEntries { get; set; }

        public Habitat Habitat { get; set; }

    }
}