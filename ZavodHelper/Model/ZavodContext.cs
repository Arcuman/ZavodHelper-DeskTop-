using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZavodHelper
{
    public class ZavodContext : DbContext
    {
        public ZavodContext() : base("ZavodConnectionSQLLITE")
        { 
        
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ZavodContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
