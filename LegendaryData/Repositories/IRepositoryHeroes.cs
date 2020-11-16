using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendaryData.Models;
namespace LegendaryData.Repositories
{
    public interface IRepositoryHeroes : IGenericRepository<Heroes>
    {
        Task<IQueryable<Heroes>> GetAllByTeam(string team);
        Task<IQueryable<Heroes>> GetAllByAuthor(string name);

    }
}
