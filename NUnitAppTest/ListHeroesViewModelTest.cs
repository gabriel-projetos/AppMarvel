using App.Interfaces;
using App.Models;
using App.Services;
using App.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NUnitAppTest
{
    //A classe a ser testada com o Nunit deve ser decorada com TextFixture.
    [TestFixture]
    public class ListHeroesViewModelTest
    {
        /*"Os testes podem mudar para o código, mas o código nunca mudará para os testes."*/
        /*Para testar precisamos pensar no resultado esperado, e não em como fazer*/
        /*precisamos pensar no resultado e nao no procesamento */

        //Um construtor atributo de configuração é escrito será executado primeiro antes de qualquer método de teste, se tive mais de um é executado de cima para baixo
        [SetUp]
        public void Setup()
        {
            
        }

        //Test: Este atributo identifica o método a ser testado
        //TearDown: será executado por último após a execução do Caso de teste., se tiver mais de um, é executado de baixo para cima
        //TestCase: é passado como passametro
        [TestCase("5")]
        [TestCase("10")]
        [TestCase("-1")]
        [TestCase("0")]
        [TestCase("100")]
        //[TearDown]
        [Test]
        public async Task Deve_Obter_A_Lista_De_Herois_Dado_A_Quantidade_Menor_Ou_Igual_A_100_E_Maior_Que_Zero(string quantidadeHerois)
        {
            var hero = new Hero
            {
                data = new Data()
            };
            hero.data.results = new List<Result>() { new Result { id = 10 } };
            //Arrang
            //Mock me disponibiliza um objeto concreto 
            var mockRepo = new Mock<IHeroes>();
            //mockRepo.Setup(hero => hero.GetHeroes(quantidadeHerois)).ReturnsAsync(hero);
            HeroesService heroesService = new HeroesService(null);
            ListHeroesViewViewModel listHeroesViewViewModel = new ListHeroesViewViewModel(null, heroesService);


            //action - ação
            await listHeroesViewViewModel.GetHeroes(quantidadeHerois);

            //assert - Comparação
            if(quantidadeHerois.Equals("5"))
                Assert.AreEqual(5, listHeroesViewViewModel.Herois.Count);
            else if (quantidadeHerois.Equals("10"))
                Assert.AreEqual(10, listHeroesViewViewModel.Herois.Count);
            else
                Assert.AreEqual(100, listHeroesViewViewModel.Herois.Count);
        }
    }
}
