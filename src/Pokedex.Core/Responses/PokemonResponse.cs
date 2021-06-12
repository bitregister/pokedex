namespace Pokedex.Core.Responses
{
    public record PokemonResponse(string Id, string Name, bool IsLegendary, string Description, string Habitat);
}
