using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData.Repositories
{
    public interface IRepositoryHenchmen : IGenericRepository<Henchmen>
    {
        Task<IQueryable<Henchmen>> GetAllByAuthor(string name);
    }
}
