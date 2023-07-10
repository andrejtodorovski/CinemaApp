using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository;
using CinemaApp.Domain.DTO;
using System.Security.Claims;
using CinemaApp.Service.Interface;

namespace CinemaApp.Web.Controllers
{
    public class ProjectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly IProjectionService _projectionService;

        public ProjectionsController(ApplicationDbContext context, ITicketService ticketService, IProjectionService projectionService)
        {
            _context = context;
            _ticketService = ticketService;
            _projectionService = projectionService;
        }

        // Преглед на сите достапни билети. Во рамките на погледот да може билетите да се филтрираат според датумот за кои истите се валидни.
        public IActionResult Index(DateTime? date)
        {
            if (date.HasValue)
            {            
                return View(_projectionService.GetAllProjections().Where(z => z.TimeOfProjection.Date.Equals(date.Value.Date)));
            }
            else
            {
                return View(_projectionService.GetAllProjections());
            }
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projection = _projectionService.GetDetailsForProjection(id);
            if (projection == null)
            {
                return NotFound();
            }

            return View(projection);
        }

        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("MovieId,TimeOfProjection,Price,TicketsAvailable,Id")] Projection projection)
        {
            if (ModelState.IsValid)
            {
                projection.Id = Guid.NewGuid();
                _projectionService.CreateNewProjection(projection);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", projection.MovieId);
            return View(projection);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projection = _projectionService.GetProjection(id);
            if (projection == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", projection.MovieId);
            return View(projection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("MovieId,TimeOfProjection,Price,TicketsAvailable,Id")] Projection projection)
        {
            if (id != projection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _projectionService.UpdateExistingProjection(projection);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectionExists(projection.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", projection.MovieId);
            return View(projection);
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projection = _projectionService.GetProjection(id);
            if (projection == null)
            {
                return NotFound();
            }

            return View(projection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var projection = _projectionService.DeleteProjection(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProjectionToCard(AddToShoppingCartDTO addToShoppingCartDTO)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _ticketService.AddToShoppingCart(addToShoppingCartDTO, userId);

            return RedirectToAction("Index", "Projections");

        }

        private bool ProjectionExists(Guid id)
        {
            return _projectionService.GetDetailsForProjection(id) != null;
        }       

    }
}
