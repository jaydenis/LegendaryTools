using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendaryData.Models;
namespace LegendaryData.Repositories
{
    public interface IRepositoryVillains: IGenericRepository<VillainGroups>
    {
        Task<IQueryable<VillainGroups>> GetAllByAuthor(string name);
    }
}
