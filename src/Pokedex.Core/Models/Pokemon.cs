namespace Pokedex.Core.Models
{
    public record Pokemon()
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsLegendary { get; set; }
    }
}