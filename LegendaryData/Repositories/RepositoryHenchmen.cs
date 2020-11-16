using LegendaryData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LegendaryData.Data.DataTools;
using LegendaryData.Models;
namespace LegendaryData.Repositories
{
    public class RepositoryHenchmen : BaseDataManager, IRepositoryHenchmen
    {
       
        public async Task<IQueryable<Henchmen>> GetAll()
        {
            var modelList = new List<Henchmen>();
            using (var db = new DataContext())
            {
                modelList = db.Henchmen.ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<Henchmen> GetOneByName(string name)
        {
            var model = new Henchmen();
            using (var db = new DataContext())
            {
                model = db.Henchmen.Where(x => x.HenchmenName == name).FirstOrDefault();
            }
            return model;
        }

        public async Task<IQueryable<Henchmen>> GetAllByAuthor(string name)
        {
            var modelList = new List<Henchmen>();
            using (var db = new DataContext())
            {
                modelList = db.Henchmen.Where(x => x.Author == name).ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task Insert(Henchmen model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.HenchmenName);
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

        public Task Update(Henchmen model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.HenchmenName);
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

        public Task Update(Henchmen model, Henchmen entity)
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
