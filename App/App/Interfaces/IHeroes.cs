using App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace App.Interfaces
{
    public interface IHeroes
    {
        Task<IEnumerable<Result>> GetHeroes(string limite);
        Task<Hero> GetHeroesWichFactory(string limite);
    }
}
