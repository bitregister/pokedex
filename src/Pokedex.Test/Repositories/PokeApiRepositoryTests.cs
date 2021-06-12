using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Pokedex.Core.Models;
using Pokedex.Core.Repositories;

namespace Pokedex.Core.Test.Repositories
{
    internal class PokeApiRepositoryTests
    {
        private PokeApiRepository _pokeApiRepository;
        private HttpTest _httpTest;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
            _pokeApiRepository = new PokeApiRepository();
        }

        [Test]
        public async Task Given_A_Pokemon_Name_It_Returns_A_Pokemon_Result()
        {
            //Arrange
            var pokemon = ConfigurePokemon();
            var expectedResponse = JsonConvert.SerializeObject(pokemon);
            _httpTest.RespondWith(expectedResponse);

            //Act
            var result = await _pokeApiRepository.GetPokemonByName("MewTwo");

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsTrue(result.IsLegendary);
            Assert.AreEqual("The Description", result.FlavorTextEntries.First().Description);
            Assert.AreEqual("en", result.FlavorTextEntries.First().Language.Name);
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }

        private static Pokemon ConfigurePokemon(string id = "1", string name = "MewTwo", bool isLegendary = true, string description = "The Description", string languageName = "en")
        {
            var language = new Language {Name = languageName};

            var flavorTextEntry = new FlavorTextEntry {Description = description, Language = language };

            var flavorTextEntryList = new List<FlavorTextEntry> {flavorTextEntry};

            var pokemon = new Pokemon
            {
                Id = id, 
                IsLegendary = isLegendary,
                Name = name,
                FlavorTextEntries = flavorTextEntryList
            };

            return pokemon;
        }

    }
}
