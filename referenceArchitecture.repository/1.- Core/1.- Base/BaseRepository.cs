using NoEstimates.Core.Helpers.DateHelper;
using NoEstimates.repository.Core.Mapper;
using referenceArchitecture.Core.Base.DTOBase;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoEstimates.repository.Core.Base
{
    public abstract class BaseRepository : IBaseRepository
    {
        //todo ARCHITECTURE include this base repository for generic crud methods and exceptions throw
        /// <summary>
        /// Primary key property name.
        /// </summary>
        public string PrimaryKeyPropertyName { get { return "Id"; } }

        /// <summary>
        /// Get a TDTO record by id.
        /// </summary>
        /// <typeparam name="TDTO">Type of the record.</typeparam>
        /// <param name="context">context of the database.</param>
        /// <param name="entity">dto that contains the id to filter by.</param>
        /// <returns>A TDTO record.</returns>
        public IQueryable<TEntity> getById<TEntity>(IDbContext context, TEntity entity) where TEntity : class
        {
            // Get id value and throw exception if id is not valid
            var idValue = (int)entity.GetType().GetProperty(PrimaryKeyPropertyName).GetValue(entity);
            throwExceptionIfPrimaryKeyInvalid(idValue);

            // Set lambda condition
            var IdParameter = Expression.Parameter(typeof(TEntity));
            var condition = Expression.Lambda<Func<TEntity, bool>>
                (
                    Expression.Equal(
                        Expression.Property(IdParameter, PrimaryKeyPropertyName),
                        Expression.Constant(idValue, typeof(int))
                    ),
                    IdParameter
                );

            // Get result as iqueryable and retur it
            var result = getAll<TEntity>(context).Where(condition);
            return result;
        }

        /// <summary>
        /// Get all the record as queryable (it does not go to db).
        /// </summary>
        /// <typeparam name="TEntity">Entity which the method is working on.</typeparam>
        /// <param name="context">Context of the database</param>
        /// <returns>A IQueryable object.</returns>
        public IQueryable<TEntity> getAll<TEntity>(IDbContext context) where TEntity : class
        {
            var query = context.Set<TEntity>().AsNoTracking().AsQueryable();
            return query;
        }

        /// <summary>
        /// Insert and save changes.
        /// </summary>
        /// <typeparam name="TEntity">Entity which the method is working on.</typeparam>
        /// <param name="context">Context of the database.</param>
        /// <param name="entityToInsert">Entity to insert.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertAndSaveChanges<TEntity>(IDbContext context, TEntity entityToInsert) where TEntity : class
        {
            context.Set<TEntity>().Add(entityToInsert);
            context.SaveChanges();
            return (int)entityToInsert.GetType().GetProperty(PrimaryKeyPropertyName).GetValue(entityToInsert);
        }

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity which the method is working on.</typeparam>
        /// <param name="context">Context of the database.</param>
        /// <param name="newEntity">Values of the new entity.</param>
        /// <param name="oldEntity">Old entity to be updated</param>
        public void update<TEntity>(IDbContext context, object newEntity, object oldEntity) where TEntity : class
        {
            // Get id value and throw exception if id is not valid
            var idValue = (int)newEntity.GetType().GetProperty(PrimaryKeyPropertyName).GetValue(newEntity);
            throwExceptionIfPrimaryKeyInvalid(idValue);

            context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <typeparam name="TEntity">Entity which the method is working on.</typeparam>
        /// <param name="context">Context of the database.</param>
        /// <param name="entityToDelete">Entity to delete.</param>
        public void delete<TEntity>(IDbContext context, TEntity entity) where TEntity : class
        {
            // Get id value and throw exception if id is not valid
            var idValue = (int)entity.GetType().GetProperty(PrimaryKeyPropertyName).GetValue(entity);
            throwExceptionIfPrimaryKeyInvalid(idValue);

            var entityToDelete = getByIdTracking<TEntity>(context, entity).FirstOrDefault();
            context.Set<TEntity>().Remove(entityToDelete);
        }

        /// <summary>
        /// Throw exception if primary key is invalid.
        /// </summary>
        /// <param name="primaryKey">Primary key.</param>
        public void throwExceptionIfPrimaryKeyInvalid(int primaryKey)
        {
            if (primaryKey <= 0) throw new PrimaryKeyIdMustBeHigherThanZero();
        }

        /// <summary>
        /// Trow exception if foreign key is invalid.
        /// </summary>
        /// <param name="foreignKey">Foreign key.</param>
        public void throwExceptionIfForeignKeyInvalid(int foreignKey)
        {
            if (foreignKey <= 0) throw new ForeignKeyIdMustBeHigherThanZero();
        }

        #region Properties
        /// <summary>
        /// Mapper property
        /// </summary>
        private IMapper mapper = DependencyResolver.Current.GetService<IMapper>();
        public IMapper Mapper
        {
            get { return mapper; }
            set { mapper = value; }
        }

        /// <summary>
        /// Helper
        /// </summary>
        private Ihp _hp = DependencyResolver.Current.GetService<Ihp>();
        public Ihp Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        /// <summary>
        /// Helper
        /// </summary>
        private IDateHp _dateHp = DependencyResolver.Current.GetService<IDateHp>();
        public IDateHp DateHp
        {
            get { return _dateHp; }
            set { _dateHp = value; }
        }
        #endregion

        /// <summary>
        /// Get by id and track the entities.
        /// </summary>
        /// <typeparam name="TEntity">TEntity to get an track.</typeparam>
        /// <param name="context">context of the database.</param>
        /// <param name="entity">Entity to get an track.</param>
        /// <returns>A single record of type TEntity as iqueryable.</returns>
        private IQueryable<TEntity> getByIdTracking<TEntity>(IDbContext context, TEntity entity) where TEntity : class
        {
            // Get id value and throw exception if id is not valid
            var idValue = (int)entity.GetType().GetProperty(PrimaryKeyPropertyName).GetValue(entity);
            throwExceptionIfPrimaryKeyInvalid(idValue);

            // Set lambda condition
            var IdParameter = Expression.Parameter(typeof(TEntity));
            var condition = Expression.Lambda<Func<TEntity, bool>>
                (
                    Expression.Equal(
                        Expression.Property(IdParameter, PrimaryKeyPropertyName),
                        Expression.Constant(idValue, typeof(int))
                    ),
                    IdParameter
                );

            // Get result as iqueryable and retur it
            var result = context.Set<TEntity>().Where(condition);
            return result;
        }
    }
}
