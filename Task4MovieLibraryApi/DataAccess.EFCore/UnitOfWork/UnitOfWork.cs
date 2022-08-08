using DataAccess.EFCore.Repositories;
using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.EFCore.UnitOfWork
{
    /// <summary>
    /// Unit of work repository pattern helps to create a separate database context for each transaction
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieContext _context;
        /// <summary>
        /// Movie repository
        /// </summary>
        public IMovieRepository<Movie> Movies { get; private set; }

        public UnitOfWork(MovieContext context)
        {
            _context = context;
            Movies = MovieRepository.GetInstance(_context);
        }
        /// <summary>
        /// Routine for saving database context
        /// </summary>
        /// <returns></returns>
        public int Complete()
        {

            return _context.SaveChanges();
        }
        /// <summary>
        /// Dispose routine
        /// </summary>
        public void Dispose()
        {
            // Dispose MovieRepository instance
            MovieRepository.GetInstance(_context).Dispose();
            // Dispose database context
            _context.Dispose();
        }
    }
}
