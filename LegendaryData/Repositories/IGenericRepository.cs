using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData.Repositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task Insert(TModel model);
        Task Update(TModel model);
        Task Update(TModel model, TModel entity);

        Task ClearData();
        Task<IQueryable<TModel>> GetAll();
        Task<TModel> GetOneByName(string name);

    }
}
