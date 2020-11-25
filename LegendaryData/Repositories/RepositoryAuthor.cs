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
    public class RepositoryAuthor : BaseDataManager, IRepositoryAuthor
    {
        public async Task<IQueryable<AuthorViewModel>> SearchByName(string name)
        {
            var modelList = new List<AuthorViewModel>();
            using (var db = new DataContext())
            {
                var entities = db.Authors.Where(x => x.AuthorName.ToLower().Contains(name)).ToList();
                foreach(var entity in entities)
                {
                    modelList.Add(Mappers.DataMap.GetMapping(entity, new AuthorViewModel()));
                }
            }
            return modelList.AsQueryable();
        }
        public async Task<IQueryable<AuthorViewModel>> GetAll()
        {
            try
            {
                var modelList = new List<AuthorViewModel>();
                using (var db = new DataContext())
                {
                    var entities = db.Authors.ToList();
                    foreach (var entity in entities)
                    {
                        modelList.Add(Mappers.DataMap.GetMapping(entity, new AuthorViewModel()));
                    }

                }
                return modelList.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.ToString());
                return null;
            }

        }

        public async Task<AuthorViewModel> GetOneByName(string name)
        {
            var model = new AuthorViewModel();
            using (var db = new DataContext())
            {
                var entity = db.Authors.Where(x => x.AuthorName.Contains(name)).FirstOrDefault();
                model = Mappers.DataMap.GetMapping(entity, new AuthorViewModel());
            }
            return model;
        }

        private async Task<Stat_Author> GetByName(string name)
        {
            var model = new Stat_Author();
            using (var db = new DataContext())
            {
                model = db.Authors.Where(x => x.AuthorName.Contains(name)).FirstOrDefault();
               
            }
            return model;
        }

        public async Task Insert(AuthorViewModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetByName(model.Name);
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


        public Task Update(AuthorViewModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = GetByName(model.Name);
                    if (entity.Result != null)
                    {
                            Mappers.DataMap.UpdateMapping(model, entity.Result);
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        
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

        public Task Update(AuthorViewModel model, Stat_Author entity)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (entity != null)
                    {
                        
                            Mappers.DataMap.UpdateMapping(model, entity);
                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        
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
