using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Pokedex.Core.Enums;
using Pokedex.Core.Models.FunTranslations;
using Pokedex.Core.Repositories;

namespace Pokedex.Core.Test.Repositories
{
    public class FunTranslationRepositoryTests
    {
        private FunTranslationsApiRepository _funTranslationsApiApiRepository;
        private HttpTest _httpTest;

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
            _funTranslationsApiApiRepository = new FunTranslationsApiRepository();
        }

        [Test]
        public async Task Given_A_Translation_To_Yoda_It_The_Expected_Translation()
        {
            //Arrange
            var expectedResponse = JsonConvert.SerializeObject(new Translation { Contents = new Contents() { Translated = "Strong with this one the force is." } });
            _httpTest.RespondWith(expectedResponse);

            //Act
            var result = await _funTranslationsApiApiRepository.GetTranslationAsync("The force is strong with this one.", TranslationEnum.Yoda);

            //Assert
            Assert.AreEqual("Strong with this one the force is.", result.Contents.Translated);
        }

        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.BadGateway)]
        public void Given_A_Pokemon_Name_That_Does_Not_Exist_Returns_FlurlHttpException(HttpStatusCode expectedStatusCode)
        {
            //Arrange
            _httpTest.RespondWith("Resource not found", (int)expectedStatusCode);

            //Assert
            Assert.ThrowsAsync<FlurlHttpException>(() => _funTranslationsApiApiRepository.GetTranslationAsync("The force is strong with this one.", TranslationEnum.Yoda));
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }
    }
}
