using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceApplication.infrastructure
{
    public interface IDbContext : IDisposable
    {
        DbSet<Example> Examples { get; set; }
        int SaveChanges();
    }
}
