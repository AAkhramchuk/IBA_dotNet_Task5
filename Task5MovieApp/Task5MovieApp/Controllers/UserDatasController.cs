using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task5MovieApp.Data;
using Task5MovieApp.Models;
using Task5MovieApp.Services;

namespace Task5MovieApp.Controllers
{
    public class UserDatasController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly MovieDBContext _context;
        private readonly UserDataViewModel _userDataViewModel;

        public UserDatasController(IMovieService movieService
                                   , MovieDBContext context
                                   , UserDataViewModel userDataViewModel)
        {
            _movieService = movieService;
            _context = context;
            _userDataViewModel = userDataViewModel;
        }

        // GET: UserDatas
        public async Task<IActionResult> Index()
        {
            return _context.UserData != null ?
                        View(await _context.UserData.ToListAsync()) :
                        Problem("Entity set 'UserDataContext.UserData'  is null.");
        }

        // GET: UserDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserData == null)
            {
                return NotFound();
            }

            var userData = await _context.UserData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userData == null)
            {
                return NotFound();
            }

            return View(userData);
        }

        // GET: UserDatas/Create
        public async Task<IActionResult> Create(int id)
        {
            SelectList genre = new SelectList(_context.Genre, "Id", "MovieGenre");
            ViewBag.Genre = genre;
            _userDataViewModel.Movie = await _movieService.GetMovie(id);
            _userDataViewModel.UserData.UserProfile.Name = User.Identity.Name;
            return View(_userDataViewModel);
        }

        // POST: UserDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieID,GenreID,UserID,UserComment,MovieRating")] UserData userData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userData);
        }

        // GET: UserDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserData == null)
            {
                return NotFound();
            }

            var userData = await _context.UserData.FindAsync(id);
            if (userData == null)
            {
                return NotFound();
            }
            return View(userData);
        }

        // POST: UserDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieID,GenreID,UserID,UserComment,MovieRating")] UserData userData)
        {
            if (id != userData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDataExists(userData.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userData);
        }

        // GET: UserDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserData == null)
            {
                return NotFound();
            }

            var userData = await _context.UserData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userData == null)
            {
                return NotFound();
            }

            return View(userData);
        }

        // POST: UserDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserData == null)
            {
                return Problem("Entity set 'UserDataContext.UserData'  is null.");
            }
            var userData = await _context.UserData.FindAsync(id);
            if (userData != null)
            {
                _context.UserData.Remove(userData);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDataExists(int id)
        {
            return (_context.UserData?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
