using DataAccess.EFCore;
using Microsoft.AspNetCore.Mvc;
using Task5MovieApp.Data;
using Task5MovieApp.Services;

namespace Task5MovieApp.Controllers
{
    public abstract class DataController : Controller
    {
        private readonly MovieContext _movieContext = new();
        private readonly MovieDBContext _movieAppContext = new();
        private readonly IMovieService _movieService;

        public MovieContext LibraryContext
        {
            get { return _movieContext; }
        }

        public MovieDBContext UserContext
        {
            get { return _movieAppContext; }
        }

        public DataController(IMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));

            ViewData["library"] = from m in LibraryContext.Movies
                                  select m;
            ViewData["userData"] = from g in UserContext.Genre
                                   select g;
        }

    }
}
