using NoEstimates.Core.Helpers.DateHelper;
using NoEstimates.repository.Core.Mapper;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.Core.Base
{
    public interface IBaseRepository
    {
        string PrimaryKeyPropertyName { get; }

        IQueryable<TEntity> getById<TEntity>(IDbContext context, TEntity entity) where TEntity : class;
        IQueryable<TEntity> getAll<TEntity>(IDbContext context) where TEntity : class;
        int insertAndSaveChanges<TEntity>(IDbContext context, TEntity entityToInsert) where TEntity : class;
        void update<TEntity>(IDbContext context, object newEntity, object oldEntity) where TEntity : class;
        void delete<TEntity>(IDbContext context, TEntity entityToDelete) where TEntity : class;
        IMapper Mapper { get; set; }
        Ihp Hp { get; set; }
        IDateHp DateHp { get; set; }
    }
}
