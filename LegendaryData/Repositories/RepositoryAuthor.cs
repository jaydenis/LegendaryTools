using LegendaryData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LegendaryData.Data.DataTools;

namespace LegendaryData.Repositories
{
    public class RepositoryAuthor : BaseDataManager, IRepositoryAuthor
    {
        public async Task<IQueryable<Stat_Author>> GetAll()
        {
            try
            {
                var modelList = new List<Stat_Author>();
                using (var db = new DataContext())
                {
                    modelList = db.Authors.ToList();

                }
                return modelList.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return null;
            }

        }

        public async Task<Stat_Author> GetOneByName(string name)
        {
            var model = new Stat_Author();
            using (var db = new DataContext())
            {
                model = db.Authors.Where(x => x.AuthorName.Contains(name)).FirstOrDefault();
            }
            return model;
        }

        public async Task Insert(Stat_Author model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.AuthorName);
                    if (entity.Result == null)
                    {
                        Mappers.DataMap.AddMapping(model, entity.Result);
                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        
                    }
                    else
                    {
                        await Update(model, entity.Result);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                throw ex;

            }
        }


        public Task Update(Stat_Author model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.AuthorName);
                    if (entity.Result != null)
                    {
                        DataCompareResult dcr = DataTools.CompareObjects(model, entity.Result);
                        if (dcr.ObjectIdentical == false)
                        {
                            Mappers.DataMap.UpdateMapping(model, entity.Result);
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        }
                    }
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                throw ex;
            }
        }

        public Task Update(Stat_Author model, Stat_Author entity)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (entity != null)
                    {
                        DataCompareResult dcr = DataTools.CompareObjects(model, entity);
                        if (dcr.ObjectIdentical == false)
                        {
                            Mappers.DataMap.UpdateMapping(model, entity);
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        }
                    }
                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                throw ex;
            }
        }


        public Task ClearData()
        {
            try
            {
                using (var db = new DataContext())
                {
                    foreach (var item in GetAll().Result)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                    }

                    return Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

    }
}
