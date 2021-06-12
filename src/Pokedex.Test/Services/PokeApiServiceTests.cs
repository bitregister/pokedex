using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Pokedex.Core.Repositories;
using Pokedex.Core.Services;
using Pokedex.Core.Test.Helpers;

namespace Pokedex.Core.Test.Services
{
    public class Tests
    {
        PokeApiService _pokeApiService;
        private Mock<IPokeApiRepository> _mockApiRepository;

        [SetUp]
        public void Setup()
        {
            _mockApiRepository = new Mock<IPokeApiRepository>();
            _pokeApiService = new PokeApiService(_mockApiRepository.Object);
        }

        [Test]
        public async Task Given_A_Pokemon_Name_It_Returns_A_PokemonResponse_Result()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon();

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);

            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo");

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsTrue(result.IsLegendary);
            Assert.AreEqual("The Description", result.Description);
        }
    }
}