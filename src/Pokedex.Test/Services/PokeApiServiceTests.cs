using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Pokedex.Core.Enums;
using Pokedex.Core.Models.FunTranslations;
using Pokedex.Core.Repositories;
using Pokedex.Core.Services;
using Pokedex.Core.Test.Helpers;

namespace Pokedex.Core.Test.Services
{
    public class Tests
    {
        PokeApiService _pokeApiService;
        private Mock<IPokeApiRepository> _mockApiRepository;
        private Mock<IFunTranslationsRepository> _mockFunTranslationRepository;

        [SetUp]
        public void Setup()
        {
            _mockApiRepository = new Mock<IPokeApiRepository>();
            _mockFunTranslationRepository = new Mock<IFunTranslationsRepository>();
            _pokeApiService = new PokeApiService(_mockApiRepository.Object, _mockFunTranslationRepository.Object);
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
        public async Task Given_A_Legendary_Pokemon_And_It_Should_Translate_It_Returns_A_Expected_PokemonResponse_Result_With_Yoda_Response()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon(habitatName: "Water");

            var translation = new Translation();
            var content = new Contents {Translated = "Description it is."};
            translation.Contents = content;

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);
            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.Is<string>(c => c.Equals("The Description")), It.Is<TranslationEnum>(e => e == TranslationEnum.Yoda))).ReturnsAsync(translation);
            
            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo", true);

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsTrue(result.IsLegendary);
            Assert.AreEqual("Description it is.", result.Description);
            Assert.AreEqual("Water", result.Habitat);
        }

        [Test]
        public async Task Given_A_Cave_Pokemon_And_It_Should_Translate_It_Returns_A_Expected_PokemonResponse_Result_With_Yoda_Response()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon(isLegendary: false);

            var translation = new Translation();
            var content = new Contents { Translated = "Description it is." };
            translation.Contents = content;

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);
            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.Is<string>(c => c.Equals("The Description")), It.Is<TranslationEnum>(e => e == TranslationEnum.Yoda))).ReturnsAsync(translation);

            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo", true);

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsFalse(result.IsLegendary);
            Assert.AreEqual("Description it is.", result.Description);
            Assert.AreEqual("Cave", result.Habitat);
        }

        [Test]
        public async Task Given_A_Water_Pokemon_And_It_Should_Translate_It_Returns_A_Expected_PokemonResponse_Result_With_Shakespeare_Response()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon(isLegendary: false, habitatName: "Water");

            var translation = new Translation();
            var content = new Contents { Translated = "Here is ye old description." };
            translation.Contents = content;

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);
            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.Is<string>(c => c.Equals("The Description")), It.Is<TranslationEnum>(e => e == TranslationEnum.Shakespeare))).ReturnsAsync(translation);

            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo", true);

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsFalse(result.IsLegendary);
            Assert.AreEqual("Here is ye old description.", result.Description);
            Assert.AreEqual("Water", result.Habitat);
        }

        [Test]
        public async Task Given_A_Water_Pokemon_And_It_Should_Translate_And_The_Translate_Service_Fails_It_Returns_A_Expected_PokemonResponse_Result_With_Original_Response()
        {
            //Arrange
            var expectedPokemon = TestHelper.ConfigurePokemon();

            var translation = new Translation();
            var content = new Contents { Translated = "Here is ye old description." };
            translation.Contents = content;

            _mockApiRepository.Setup(pokeApiRepository => pokeApiRepository.GetPokemonByNameAsync(It.Is<string>(s => s.Equals("MewTwo")))).ReturnsAsync(expectedPokemon);
            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<TranslationEnum>())).ThrowsAsync(new Exception());
            
            //Act
            var result = await _pokeApiService.GetPokemonByNameAsync("MewTwo", true);

            //Assert
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("MewTwo", result.Name);
            Assert.IsTrue(result.IsLegendary);
            Assert.AreEqual("The Description", result.Description);
            Assert.AreEqual("Cave", result.Habitat);
        }
    }
}