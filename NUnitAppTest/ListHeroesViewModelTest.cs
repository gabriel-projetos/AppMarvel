using App.Interfaces;
using App.Models;
using App.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NUnitAppTest
{
    [TestFixture]
    public class ListHeroesViewModelTest
    {
        /*"Os testes podem mudar para o código, mas o código nunca mudará para os testes."*/

        //Um construtor
        [SetUp]
        public void Setup()
        {
            
        }

        //[TestCase("5", "10")]
        [Test]
        public async Task DeveObterAListaDeHerois()
        {
            Hero hero = new Hero();
            var mockRepo = new Mock<IHeroes>();
            mockRepo.Setup(x => x.GetHeroes("5")).ReturnsAsync(hero) ;

            ListHeroesViewViewModel listHeroesViewViewModel = new ListHeroesViewViewModel(null, mockRepo.Object);
            await listHeroesViewViewModel.GetHeroes("5");


            List<Hero> listaHeroes = new List<Hero>() 
            {
                new Hero(),
                new Hero(),
                new Hero(),
                new Hero(),
                new Hero()
            };
            
            Assert.AreEqual(listaHeroes.Count, listHeroesViewViewModel.Herois.Count);
        }
    }
}
