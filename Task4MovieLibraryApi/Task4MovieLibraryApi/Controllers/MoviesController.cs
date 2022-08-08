using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.EFCore;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Entities.Wrappers;

namespace Task4MovieLibraryApi.Controllers
{
    /// <summary>
    /// Create requests to the database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("movieAPI")]
    public class MoviesController : ControllerBase
    {
        // Error message constants
        private const string _emptyDBset = "Entity set 'Movies' is null.";
        private const string _notFound   = "Record not found.";
        private const string _badRequest = "Bad request.";

        private readonly MovieContext _MovieContext;
        private readonly IUriService _UriService;
        private readonly Response<Movie> _responseMovie;
        private readonly Response<List<Movie>> _responseMovieList;
        private readonly IPagingRepository<Movie> _PagingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MoviesController(MovieContext movieContext
                                , IUriService uriService
                                , Response<Movie> responseMovie
                                , Response<List<Movie>> responseMovieList
                                , IPagingRepository<Movie> pagingRepository
                                , IUnitOfWork unitOfWork)
        {
            _MovieContext       = movieContext;
            _UriService         = uriService;
            _responseMovie      = responseMovie;
            _responseMovieList  = responseMovieList;
            _PagingRepository   = pagingRepository;
            _unitOfWork         = unitOfWork;
            _responseMovieList  = responseMovieList;
        }

        /// <summary>
        /// GET: api/Movies
        /// Create request and prepair recieved data by pages according PagingFilter parametes
        /// </summary>
        /// <param name="filter">Filter parameters</param>
        /// <returns>Action result</returns>
        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<Movie>>> GetMovies([FromQuery] PagingFilter filter)
        {
            if (_MovieContext.Movies == null)
            {
                return Problem(_emptyDBset);
            }
            string route = Request.Path.Value;
            PagingFilter validFilter = new (filter.PageNumber, filter.PageSize);
            var pagedData = await _MovieContext.Movies
                .OrderBy(m => m.ID)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            int totalRecords = await _MovieContext.Movies.CountAsync();
            var pagedReponse = _PagingRepository.CreatePagedReponse(pagedData
                                                                    , validFilter
                                                                    , totalRecords
                                                                    , _UriService
                                                                    , route);

            return Ok(pagedReponse);
        }

        /// <summary>
        /// GET: api/Movies/5
        /// Get any Movie record by ID
        /// </summary>
        /// <param name="id">Record ID in the database</param>
        /// <returns>Action result</returns>
        [HttpGet("{id}")]
        public async ValueTask<ActionResult<Movie>> GetMovie(int id)
        {
            if (_MovieContext.Movies == null)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = null;
                _responseMovie.Message = _emptyDBset;
                return Ok(_responseMovie);
            }
            var movie = await _MovieContext.Movies.FindAsync(id);
            if (movie == null)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = movie;
                _responseMovie.Message = _notFound;
                return Ok(_responseMovie);
            }

            _responseMovie.Data = movie;
            return Ok(_responseMovie);
        }

        /// <summary>
        /// Get exact number of records with the highest Movie raiting
        /// </summary>
        /// <param name="count">The number of records to be returned</param>
        /// <returns>Action result</returns>
        public async ValueTask<ActionResult<List<Movie>>> GetPopularMovies([FromQuery] int count)
        {
            _responseMovieList.Data = await _unitOfWork.Movies.GetPopularMovies(count);
            return Ok(_responseMovieList);
        }

        /// <summary>
        /// PUT: api/Movies/5
        /// Update a record in the database table
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <param name="movie">Record data</param>
        /// <returns>Action result</returns>
        [HttpPut("{id}")]
        public async ValueTask<ActionResult<Movie>> PutMovie(int id, Movie movie)
        {
            if (_MovieContext.Movies == null)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = null;
                _responseMovie.Message = _emptyDBset;
                return Ok(_responseMovie);
            }
            if (id != movie.ID)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = movie;
                _responseMovie.Message = _badRequest;
                return Ok(_responseMovie);
            }
            _MovieContext.Entry(movie).State = EntityState.Modified;
            try
            {
                await _MovieContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    _responseMovie.Succeeded = false;
                    _responseMovie.Data = null;
                    _responseMovie.Message = _notFound;
                    return Ok(_responseMovie);
                }
                else
                {
                    throw;
                }
            }

            _responseMovie.Data = movie;
            return Ok(_responseMovie);
        }

        /// <summary>
        /// POST: api/Movies
        /// Add new record to the database table
        /// </summary>
        /// <param name="movie">Record data</param>
        /// <returns>Action result</returns>
        [HttpPost]
        public async ValueTask<ActionResult<Movie>> PostMovie(Movie movie)
        {
            if (_MovieContext.Movies == null)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = null;
                _responseMovie.Message = _emptyDBset;
                return Ok(_responseMovie);
            }
            _MovieContext.Movies.Add(movie);
            await _MovieContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { ID = movie.ID }, movie);
        }

        /// <summary>
        /// DELETE: api/Movies/5
        /// Delete a record by ID number
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Action result</returns>
        [HttpDelete("{id}")]
        public async ValueTask<ActionResult<Movie>> DeleteMovie(int id)
        {
            if (_MovieContext.Movies == null)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = null;
                _responseMovie.Message = _emptyDBset;
                return Ok(_responseMovie);
            }
            var movie = await _MovieContext.Movies.FindAsync(id);
            if (movie == null)
            {
                _responseMovie.Succeeded = false;
                _responseMovie.Data = movie;
                _responseMovie.Message = _notFound;
                return Ok(_responseMovie);
            }
            _MovieContext.Movies.Remove(movie);
            await _MovieContext.SaveChangesAsync();
            movie = await _MovieContext.Movies.FindAsync(id);

            _responseMovie.Data = movie;
            return Ok(_responseMovie);
        }

        /// <summary>
        /// Determine a record existence by ID number
        /// </summary>
        /// <param name="id">Record ID</param>
        /// <returns>Action result</returns>
        private bool MovieExists(int id)
        {
            return (_MovieContext.Movies?.Any(m => m.ID == id)).GetValueOrDefault();
        }
    }
}
