using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Pokedex.Core.Models.FunTranslations;
using Pokedex.Core.Repositories;
using Pokedex.Core.Responses;
using Pokedex.Core.Services;
using Pokedex.Core.Services.Translation;
using Pokedex.Core.Test.Helpers;

namespace Pokedex.Core.Test.Services
{
    public class Tests
    {
        private PokeApiService _pokeApiService;
        private Mock<IPokeApiRepository> _mockApiRepository;
        private Mock<ITranslationService> _mockTranslationService;
        
        [SetUp]
        public void Setup()
        {
            _mockApiRepository = new Mock<IPokeApiRepository>();
            _mockTranslationService = new Mock<ITranslationService>();
            _pokeApiService = new PokeApiService(_mockApiRepository.Object, _mockTranslationService.Object);
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
            Assert.AreEqual("Cave", result.Habitat);
        }

        [Test]
        public async Task Given_A_Pokemon_Name_And_It_Returns_A_PokemonResponse_WithNullHabitat_Returns_Empty_Value()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon(habitatName: null);

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);

            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo");

            //Assert
            Assert.AreEqual("", result.Habitat);
        }
        
        [Test]
        public async Task Given_A_Pokemon_And_We_Should_Translate_It_Should_Translate_The_Description()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon(isLegendary: false);

            var translation = new Translation();
            var content = new Contents { Translated = "Description it is." };
            translation.Contents = content;

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);
            _mockTranslationService.Setup(x => x.GetTranslation(It.IsAny<PokemonResponse>())).ReturnsAsync("The Description it is.");
            
            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo", true);

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsFalse(result.IsLegendary);
            Assert.AreEqual("The Description it is.", result.Description);
            Assert.AreEqual("Cave", result.Habitat);
        }
    }
}