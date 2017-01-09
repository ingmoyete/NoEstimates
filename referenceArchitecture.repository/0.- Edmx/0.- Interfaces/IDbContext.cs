using NoEstimates.repository._0.__Edmx;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.repository.Edmx.Interfaces
{
    public interface IDbContext : IDisposable
    {
        int SaveChanges();
        Database Database { get; }
        DbEntityEntry Entry(Object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet Set(Type entityType);
        DbSet<Category> Categories { get; set; }
        DbSet<Configuration> Configurations { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Requirement> Requirements { get; set; }
        DbSet<NoEstimates.repository._0.__Edmx.Task> Tasks { get; set; }
        DbSet<TasksCategory> TasksCategories { get; set; }

        DbSet<Complete> Completes { get; set; }
        DbSet<Highlight> Highlights { get; set; }
        DbSet<Timer> Timers { get; set; }

    }
}
