using NUnit.Framework;
using Pokedex.Core.Services;

namespace Pokedex.Test
{
    public class Tests
    {
        PokeApiService _pokeApiService;

        [SetUp]
        public void Setup()
        {
            _pokeApiService = new PokeApiService();
        }

        [Test]
        public void Given_A_Pokemon_Name_It_Returns_A_PokemonResponse_Result()
        {
            //Act
            var result = _pokeApiService.GetPokemonByName("mewtwo");

            //Assert
            Assert.AreEqual("1", result.Id);
        }
    }
}