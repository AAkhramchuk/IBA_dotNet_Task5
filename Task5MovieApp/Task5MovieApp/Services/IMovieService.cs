using Task5MovieApp.Models;
using Domain.Entities;
using Domain.Entities.Wrappers;

namespace Task5MovieApp.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>?> GetMovies();

        Task<Movie?> GetMovie(int id);

        Task<Movie?> CreateMovie(Movie movie);

        Task<Movie?> UpdateMovie(int id, Movie movie);

        Task<Movie?> DeleteMovie(int id);

        Task<string> GetResponse(string route, HttpMethod method, Movie? movie = null);

        Task<UserInfoViewModel> GetUserInfo();
    }
}
