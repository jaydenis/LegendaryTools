using LegendaryData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LegendaryData.Data.DataTools;

namespace LegendaryData.Repositories
{
    public class RepositoryMasterminds : BaseDataManager, IRepositoryMasterminds
    {
        private string GetKey(Masterminds model)
        {
            return CreateMD5($"{model.MastermindName}{model.Author}{model.UniverseName}{model.BGGLink}{model.ExpansionName}");
        }
        public async Task<IQueryable<Masterminds>> GetAll()
        {
            var modelList = new List<Masterminds>();
            using (var db = new DataContext())
            {
                modelList = db.Masterminds.ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<IQueryable<Masterminds>> GetAllBySet(List<long> ids)
        {
            var modelList = new List<Masterminds>();
            using (var db = new DataContext())
            {
               // modelList = db.Masterminds.Where(x => ids.Contains((long)x.SetID)).ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<IQueryable<Masterminds>> GetAllByAuthor(string name)
        {
            var modelList = new List<Masterminds>();
            using (var db = new DataContext())
            {
                modelList = db.Masterminds.Where(x => x.Author == name).ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<Masterminds> GetOneById(long id)
        {
            var model = new Masterminds();
            using (var db = new DataContext())
            {
                model = db.Masterminds.Where(x => x.ID == id).FirstOrDefault();
            }
            return model;
        }

        public async Task<Masterminds> GetOneByName(string name)
        {
            var model = new Masterminds();
            using (var db = new DataContext())
            {
                model = db.Masterminds.Where(x => x.MastermindName.Contains(name)).FirstOrDefault();
            }
            return model;
        }

        public async Task Insert(Masterminds model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var key = GetKey(model);
                    var entity = db.Masterminds.Where(x => x.Key == key).FirstOrDefault();
                    if (entity == null)
                    {
                        model.Key = key;
                        Mappers.DataMap.AddMapping(model, entity);
                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        
                    }
                    else
                    {
                        await Update(model, entity);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        public Task Update(Masterminds model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.MastermindName);
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
                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        public Task Update(Masterminds model, Masterminds entity)
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
                _logger.Error(ex, ex.Message);
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
