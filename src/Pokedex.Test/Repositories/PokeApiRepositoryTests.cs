using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using NUnit.Framework;
using Pokedex.Core.Repositories;
using Pokedex.Core.Services;

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
            _httpTest.RespondWithJson(new {Id = 1});

            //Act
            var result = await _pokeApiRepository.GetPokemonByName("MewTwo");

            //Assert
            Assert.AreEqual("1", result.Id);
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }
    }
}
