using Domain.Interfaces;
using System.Linq.Expressions;

namespace DataAccess.EFCore.Repositories
{
    /// <summary>
    /// Generic functions to work with any database context
    /// </summary>
    /// <typeparam name="T">Any database context type</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MovieContext _context;
        public GenericRepository(MovieContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Adds an entity to the database context
        /// </summary>
        /// <param name="entity">Data entity</param>
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        /// <summary>
        /// Adds range of entities to the database context
        /// </summary>
        /// <param name="entities">A range of entities</param>
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        /// <summary>
        /// Filters the database context with where expression
        /// </summary>
        /// <param name="expression">Filter expression</param>
        /// <returns>Any portion of enumerable entities according filter</returns>
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        /// <summary>
        /// Get whole list of entities from the database context
        /// </summary>
        /// <returns>Whole list of entities with out filter</returns>
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        /// <summary>
        /// Get any entity by ID from the database context
        /// </summary>
        /// <param name="id">ID variable</param>
        /// <returns>Any entity from database context</returns>
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        /// <summary>
        /// Remove any entity by ID from the database context
        /// </summary>
        /// <param name="entity">Any entity from database context for removing</param>
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        /// <summary>
        /// Remove any enumerable range of entities from the database context
        /// </summary>
        /// <param name="entities">Enumerable range of entities for removing</param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}

