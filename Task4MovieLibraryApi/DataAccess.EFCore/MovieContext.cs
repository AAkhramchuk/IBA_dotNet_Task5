using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore
{
    /// <summary>
    /// Movie data context
    /// </summary>
    public class MovieContext : DbContext
    {
        public MovieContext() : base() { }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

        public MovieContext? GetMovieContext { get; }

        /// <summary>
        /// Used to query and save Movie instances
        /// </summary>
        public DbSet<Movie> Movies { get { return Set<Movie>(); } set { } }
    }
}
