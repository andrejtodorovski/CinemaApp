using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace CinemaApp.Web.Controllers
{

    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;


        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index()
        {
            return View(_movieService.GetAllMovies());
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("MovieTitle,MovieDescription,MovieGenre,MovieDirector,MovieCast,MovieDuration,Id")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieService.CreateNewMovie(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("MovieTitle,MovieDescription,MovieGenre,MovieDirector,MovieCast,MovieDuration,Id")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _movieService.UpdeteExistingMovie(movie);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _movieService.GetDetailsForMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var movie = _movieService.DeleteMovie(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(Guid id)
        {
            return _movieService.GetAllMovies().Any(e => e.Id == id);
        }
    }
}
