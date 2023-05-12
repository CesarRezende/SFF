﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace SFF.Infra.Repository.Base
{
    public abstract class BaseEntityRepository<TEntity>: BaseRepository where TEntity : class
    {
        protected BaseEntityRepository(
            SFFDbContext dbContext,
            ILogger<BaseEntityRepository<TEntity>> logger): base(dbContext, logger)
        {

        }


        private DbSet<TEntity> _dbSet;

        protected DbContext Context
        {
            get
            {
                return _dbContext;
            }
        }

        protected DbSet<TEntity> DBSet
        {
            get
            {
                if (_dbSet == null)
                {
                    _dbSet = this.Context.Set<TEntity>();
                }

                return _dbSet;
            }
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderExpression = null)
        {
            return this.GetQuery(predicate, orderExpression).AsEnumerable();
        }

        public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderExpression = null)
        {
            IQueryable<TEntity> qry = this.DBSet;

            if (predicate != null)
                qry = qry.Where(predicate);

            if (orderExpression != null)
                return orderExpression(qry);


            return qry;
        }

        public virtual void Insert<T>(T entity) where T : class
        {
            DbSet<T> dbSet = this.Context.Set<T>();
            dbSet.Add(entity);
        }

        public virtual void Insert(TEntity entity)
        {
            this.DBSet.Add(entity);
        }

        public virtual void Update<T>(T entity) where T : class
        {
            DbSet<T> dbSet = this.Context.Set<T>();
            dbSet.Attach(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity)
        {
            this.Attach(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            DbSet<T> dbSet = this.Context.Set<T>();

            if (this.Context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);

        }

        public virtual void Delete(TEntity entity)
        {
            if (this.Context.Entry(entity).State == EntityState.Detached)
                this.Attach(entity);

            this.DBSet.Remove(entity);

        }

        public virtual void Delete<T>(object[] id) where T : class
        {
            DbSet<T> dbSet = this.Context.Set<T>();
            T entity = dbSet.Find(id);
            dbSet.Attach(entity);
            dbSet.Remove(entity);

        }

        public virtual void Delete(object id)
        {
            TEntity entity = this.DBSet.Find(id);
            this.Delete(entity);
        }


        public virtual void Attach(TEntity entity)
        {
            if (this.Context.Entry(entity).State == EntityState.Detached)
                this.DBSet.Attach(entity);
        }

        public virtual void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
