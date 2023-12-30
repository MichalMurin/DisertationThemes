using DissertationThemes.SharedLibrary.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.SharedLibrary.DataBase
{
    /// <summary>
    /// DbContext class for interacting with DB
    /// </summary>
    public class ThemesDbContext : DbContext
    {
        public DbSet<Theme> Theme { get; set; }
        public DbSet<Supervisor> Supervisor { get; set; }
        public DbSet<StProgram> StProgram { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CommonDeffinitions.DbConnectionString);
        }
    }
}
