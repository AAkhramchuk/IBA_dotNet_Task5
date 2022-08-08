namespace Domain.Interfaces
{
    public interface IMovieRepository<T>// : IGenericRepository<T> where T : class
    {
        ValueTask<List<T>> GetPopularMovies(int count);
    }
}
