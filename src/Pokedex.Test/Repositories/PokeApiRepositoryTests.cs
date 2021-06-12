using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Pokedex.Core.Repositories;
using Pokedex.Core.Test.Helpers;

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
            var pokemon = TestHelper.ConfigurePokemon();
            var expectedResponse = JsonConvert.SerializeObject(pokemon);
            _httpTest.RespondWith(expectedResponse);

            //Act
            var result = await _pokeApiRepository.GetPokemonByNameAsync("MewTwo");

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsTrue(result.IsLegendary);
            Assert.AreEqual("The Description", result.FlavorTextEntries.First().Description);
            Assert.AreEqual("en", result.FlavorTextEntries.First().Language.Name);
            Assert.AreEqual("Cave", result.Habitat.Name);
        }

        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadGateway)]
        public void Given_A_Pokemon_Name_That_Does_Not_Exist_Returns_FlurlHttpException(HttpStatusCode expectedStatusCode)
        {
            //Arrange
            _httpTest.RespondWith("Resource not found", (int)expectedStatusCode);

            //Assert
            Assert.ThrowsAsync<FlurlHttpException>(() => _pokeApiRepository.GetPokemonByNameAsync("MewThree"));
        }
        
        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }
    }
}
