using System.Linq;
using System.Threading.Tasks;

namespace LegendaryData.Repositories
{
    public interface IRepositoryMasterminds : IGenericRepository<Masterminds>
    {
        Task<IQueryable<Masterminds>> GetAllByAuthor(string name);
    }
}