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
    public class RepositoryTeams : BaseDataManager, IRepositoryTeams
    {
        public async Task<IQueryable<Stat_Affiliation>> GetAll()
        {
            var modelList = new List<Stat_Affiliation>();
            using (var db = new DataContext())
            {
                modelList = db.Teams.ToList();
            }
            return modelList.AsQueryable();
        }

        public async Task<IQueryable<Stat_Affiliation>> GetAllBySet(List<long> ids)
        {
            var modelList = new List<Stat_Affiliation>();
            using (var db = new DataContext())
            {
                modelList = db.Teams.Where(x => ids.Contains(x.ID)).ToList();
            }
            return modelList.AsQueryable();
        }


        public async Task<Stat_Affiliation> GetOneById(long id)
        {
            var model = new Stat_Affiliation();
            using (var db = new DataContext())
            {
                model = db.Teams.Where(x => x.ID == id).FirstOrDefault();
            }
            return model;
        }

        public async Task<Stat_Affiliation> GetOneByName(string name)
        {
            var model = new Stat_Affiliation();
            using (var db = new DataContext())
            {
               model = db.Teams.Where(x => x.TeamName.Contains(name)).FirstOrDefault();
            }
            return model;
        }

        public async Task Insert(Stat_Affiliation model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.TeamName);
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


        public Task Update(Stat_Affiliation model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetOneByName(model.TeamName);
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

        public Task Update(Stat_Affiliation model, Stat_Affiliation entity)
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
