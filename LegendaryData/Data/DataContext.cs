using LegendaryData.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendaryData.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        : base("MyDBContext")
        {

        }
        public DbSet<Heroes> Heroes { get; set; }
        public DbSet<Stat_Author> Authors { get; set; }
        public DbSet<Stat_Affiliation> Teams { get; set; }
        public DbSet<Masterminds> Masterminds { get; set; }
        public DbSet<VillainGroups> Villains { get; set; }
        public DbSet<Henchmen> Henchmen { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
