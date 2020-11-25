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
    public class RepositoryHeroes : BaseDataManager, IRepositoryHeroes
    {
        public async Task<IQueryable<Heroes>> SearchByName(string name)
        {
            var modelList = new List<Heroes>();
            using (var db = new DataContext())
            {
                modelList = db.Heroes.Where(x => x.HeroName.ToLower().Contains(name)).ToList();
            }
            return modelList.AsQueryable();
        }
        private string GetKey(Heroes model)
        {
            return CreateMD5($"{model.HeroName}{model.Author}{model.TeamName}{model.Universe}{model.ExpansionName}{model.BGGLink}");
        }

        public async Task<IQueryable<Heroes>> GetAll()
        {
            try
            {
                var modelList = new List<Heroes>();
                using (var db = new DataContext())
                {
                    modelList = db.Heroes.ToList();

                }
                return modelList.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return null;
            }

        }

        public async Task<IQueryable<Heroes>> GetAllBySet(List<long> ids)
        {
            var modelList = new List<Heroes>();
            using (var db = new DataContext())
            {
                //modelList = db.Heroes.Where(x => ids.Contains((long)x.SetID)).ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<IQueryable<Heroes>> GetAllByTeam(string name)
        {
            var modelList = new List<Heroes>();
            using (var db = new DataContext())
            {
                modelList = db.Heroes.Where(x => x.TeamName == name).ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<IQueryable<Heroes>> GetAllByAuthor(string name)
        {
            var modelList = new List<Heroes>();
            using (var db = new DataContext())
            {
                  modelList = db.Heroes.Where(x => x.Author == name).ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<Heroes> GetOneById(long id)
        {
            var model = new Heroes();
            using (var db = new DataContext())
            {
                model = db.Heroes.Where(x => x.ID == id).FirstOrDefault();
            }
            return model;
        }

        public async Task<Heroes> GetOneByName(string name)
        {
            var model = new Heroes();
            using (var db = new DataContext())
            {
                model = db.Heroes.Where(x => x.HeroName.Contains(name)).FirstOrDefault();
            }
            return model;
        }

        public async Task Insert(Heroes model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var key = GetKey(model);
                    var entity = db.Heroes.Where(x => x.Key == key).FirstOrDefault();
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

        public Task Update(Heroes model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.HeroName);
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

        public Task Update(Heroes model, Heroes entity)
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
