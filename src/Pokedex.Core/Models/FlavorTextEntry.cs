using Newtonsoft.Json;

namespace Pokedex.Core.Models
{
    public record FlavorTextEntry
    {
        [JsonProperty("Flavor_Text")]
        public string Description { get; set; }

        public Language Language { get; set; }
    }
}
