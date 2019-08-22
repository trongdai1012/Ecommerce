using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KLTN.Services.Repositories
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities of type T
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get entity of type T
        /// </summary>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets entities using delegate
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IEnumerable<T> GetMany(Func<T, bool> where);

        /// <summary>
        /// Gets entities using delegate
        /// </summary>
        IQueryable<T> ObjectContext { get; set; }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(object id);

        /// <summary>
        /// Marks an entity as new
        /// </summary>
        /// <param name="entity"></param>
        T Create(T entity);

        /// <summary>
        /// Marks an entity as modified
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Marks an enity to be remove by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        ///  Marks an enity to be remove range
        /// </summary>
        /// <param name="entities"></param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Save change
        /// </summary>
        void Save();
    }
}
