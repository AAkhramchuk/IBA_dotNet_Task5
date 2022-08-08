using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Task5MovieApp.Services;
using Domain.Entities;
using System.Security.Claims;
using Domain.Entities.Wrappers;
using Newtonsoft.Json;
using IdentityModel.Client;
using IdentityServer4;

namespace Task5MovieApp.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            await LogTokenAndClaims();
            return View(await _movieService.GetMovies());
        }
        public async Task LogTokenAndClaims()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token: {identityToken}");

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> OnlyAdmin()
        {
            var userInfo = await _movieService.GetUserInfo();
            return View(userInfo);
        }


        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await _movieService.GetMovie(id));
            //return View();

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var movie = await _context.Movie
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            //return View(movie);
        }

        // GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            //return View(await _movieService.CreateMovie(movie));
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed([Bind("ID,ProducerName,ProducerSurname,MovieName,MovieYear,MovieRating")] Movie movie)
        {
            await LogTokenAndClaims();
            return View(await _movieService.CreateMovie(movie));

            //if (ModelState.IsValid)
            //{
            //    _context.Add(movie);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _movieService.GetMovie(id));

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var movie = await _context.Movie.FindAsync(id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}
            //return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("ID,ProducerName,ProducerSurname,MovieName,MovieYear,MovieRating")] Movie movie)
        {
            //return View();
            await LogTokenAndClaims();
            return View(await _movieService.UpdateMovie(id, movie));

            //if (id != movie.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(movie);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!MovieExists(movie.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)//, [Bind("ID,ProducerName,ProducerSurname,MovieName,MovieYear,MovieRating")] Movie movie)
        {
            return View(await _movieService.GetMovie(id));
            //return View();

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var movie = await _context.Movie
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            //return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("ID,ProducerName,ProducerSurname,MovieName,MovieYear,MovieRating")] Movie movie)
        {
            return View(await _movieService.DeleteMovie(id));
            //return View();

            //var movie = await _context.Movie.FindAsync(id);
            //_context.Movie.Remove(movie);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        /*
        private bool MovieExists(int id)
        {
            return true;

            //return _context.Movie.Any(e => e.Id == id);
        }
        */
    }
}
