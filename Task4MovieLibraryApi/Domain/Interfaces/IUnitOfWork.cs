using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository<Movie> Movies { get; }
        int Complete();
    }
}
