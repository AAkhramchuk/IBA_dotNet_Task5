using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore.Repositories
{
    /// <summary>
    /// Movie repository functions.
    /// Implements singleton pattern because of
    /// dividing database requests logic to different transactions (UnitOfWork).
    /// </summary>
    public class MovieRepository :GenericRepository<Movie>, IMovieRepository<Movie>
    {
        private static MovieContext _context;

        private static MovieRepository? _instance = null;
        private static object _mutex = new();
        private MovieRepository(MovieContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates new instance if it is not exist
        /// </summary>
        /// <param name="context">Movie database context</param>
        /// <returns>MovieRepository class instance</returns>
        public static MovieRepository GetInstance(MovieContext context)
        {
            if (_instance == null)
            {
                lock (_mutex) // Some form of thread safety
                {
                    if (_instance == null)
                    {
                        _instance = new MovieRepository(context);
                    }
                }
            }

            return _instance;
        }

        /// <summary>
        /// Get defined number of records with the highest Movie raiting
        /// </summary>
        /// <param name="count">The number of records to be returned</param>
        /// <returns>The list of Movie records</returns>
        public async ValueTask<List<Movie>> GetPopularMovies(int count)
        {
            return await _context.Movies.OrderByDescending(m => m.MovieRating).Take(count).ToListAsync();
        }

        /// <summary>
        /// Disposes an unnecessary instance because
        /// the database context is also deleted.
        /// "Unit of work" logic.
        /// </summary>
        public void Dispose()
        {
            _instance = null;
            GC.Collect();
        }
    }
}
