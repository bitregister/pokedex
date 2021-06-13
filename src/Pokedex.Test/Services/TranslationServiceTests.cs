using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Pokedex.Core.Enums;
using Pokedex.Core.Models.FunTranslations;
using Pokedex.Core.Repositories;
using Pokedex.Core.Responses;
using Pokedex.Core.Services.Translation;

namespace Pokedex.Core.Test.Services
{
    public class TranslationServiceTests
    {
        private Mock<IFunTranslationsRepository> _mockFunTranslationRepository;
        private Mock<ITranslationStrategyFactory> _mockTranslationStrategyFactory;
        private ITranslationService _translationService;

        [SetUp]
        public void Setup()
        {   
            _mockFunTranslationRepository = new Mock<IFunTranslationsRepository>();
            _mockTranslationStrategyFactory = new Mock<ITranslationStrategyFactory>();
            _translationService = new TranslationService(_mockFunTranslationRepository.Object, _mockTranslationStrategyFactory.Object);
        }

        [Test]
        public async Task Given_A_Non_Legendary_Water_Pokemon_And_It_Should_Translate_It_With_Shakespeare_Response()
        {
            //Arrange
            var pokemonResponse = new PokemonResponse("1", "Slowking", false, "The description", "Water");

            var translation = new Translation();
            var content = new Contents { Translated = "Here is ye old description." };
            translation.Contents = content;

            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.Is<TranslationEnum>(e => e == TranslationEnum.Shakespeare))).ReturnsAsync(translation);
            _mockTranslationStrategyFactory.Setup(x => x.GetTranslationStrategy(It.Is<TranslationEnum>(c => c == TranslationEnum.Shakespeare), It.IsAny<IFunTranslationsRepository>())).Returns(new ShakespeareTranslationStrategy(_mockFunTranslationRepository.Object));
            

            //Act
            var result = await _translationService.GetTranslation(pokemonResponse);

            //Assert
            Assert.AreEqual("Here is ye old description.", result);
            _mockTranslationStrategyFactory.Verify();
        }

        [Test]
        public async Task Given_A_Legendary_Water_Pokemon_And_It_Should_Translate_It_With_Yoda_Response()
        {
            //Arrange
            var pokemonResponse = new PokemonResponse("1", "Slowking", true, "The description", "Water");

            var translation = new Translation();
            var content = new Contents { Translated = "Description, here it is." };
            translation.Contents = content;

            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.Is<TranslationEnum>(e => e == TranslationEnum.Shakespeare))).ReturnsAsync(translation);
            _mockTranslationStrategyFactory.Setup(x => x.GetTranslationStrategy(It.Is<TranslationEnum>(c => c == TranslationEnum.Yoda), It.IsAny<IFunTranslationsRepository>())).Returns(new ShakespeareTranslationStrategy(_mockFunTranslationRepository.Object));
            
            //Act
            var result = await _translationService.GetTranslation(pokemonResponse);

            //Assert
            Assert.AreEqual("Description, here it is.", result);
            _mockTranslationStrategyFactory.Verify();
        }

        [Test]
        public async Task Given_A_Non_Legendary_Cave_Pokemon_And_It_Should_Translate_It_With_Yoda_Response()
        {
            //Arrange
            var pokemonResponse = new PokemonResponse("1", "Slowking", false, "The description", "Cave");

            var translation = new Translation();
            var content = new Contents { Translated = "Description, here it is." };
            translation.Contents = content;

            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.Is<TranslationEnum>(e => e == TranslationEnum.Shakespeare))).ReturnsAsync(translation);
            _mockTranslationStrategyFactory.Setup(x => x.GetTranslationStrategy(It.Is<TranslationEnum>(c => c == TranslationEnum.Yoda), It.IsAny<IFunTranslationsRepository>())).Returns(new ShakespeareTranslationStrategy(_mockFunTranslationRepository.Object));

            //Act
            var result = await _translationService.GetTranslation(pokemonResponse);

            //Assert
            Assert.AreEqual("Description, here it is.", result);
            _mockTranslationStrategyFactory.Verify();
        }

        [Test]
        public async Task Given_A_Pokemon_And_It_Should_Translate_And_The_Translate_Service_Fails_It_Returns_A_Expected_PokemonResponse_Result_With_Original_Response()
        {
            //Arrange
            var pokemonResponse = new PokemonResponse("1", "Slowking", false, "The description", "Water");

            var translation = new Translation();
            var content = new Contents { Translated = "Description, here it is." };
            translation.Contents = content;

            _mockFunTranslationRepository.Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<TranslationEnum>())).ThrowsAsync(new Exception());
            _mockTranslationStrategyFactory.Setup(x => x.GetTranslationStrategy(It.IsAny<TranslationEnum>(), It.IsAny<IFunTranslationsRepository>())).Returns(new ShakespeareTranslationStrategy(_mockFunTranslationRepository.Object));

            //Act
            var result = await _translationService.GetTranslation(pokemonResponse);

            //Assert
            Assert.AreEqual("The description", result);
        }
    }
}
