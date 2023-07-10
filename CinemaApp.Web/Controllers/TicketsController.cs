using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using CinemaApp.Service.Interface;

namespace CinemaApp.Web.Controllers
{
    // Сите CRUD операции (Додавање, Едитирање, Бришење) поврзани со билети.
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;

        public TicketsController(ApplicationDbContext context, ITicketService ticketService)
        {
            _context = context;
            _ticketService = ticketService;
        }

        public IActionResult Index()
        {
            return View(_ticketService.GetAllTickets());
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        public IActionResult Create()
        {
            ViewData["ProjectionId"] = new SelectList(_context.Projections, "Id", "Id");
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProjectionId,Quantity,ShoppingCartId,Id")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectionId"] = new SelectList(_context.Projections, "Id", "Id", ticket.ProjectionId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "Id", "Id", ticket.ShoppingCartId);
            return View(ticket);
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ProjectionId"] = new SelectList(_context.Projections, "Id", "Id", ticket.ProjectionId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "Id", "Id", ticket.ShoppingCartId);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ProjectionId,Quantity,ShoppingCartId,Id")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["ProjectionId"] = new SelectList(_context.Projections, "Id", "Id", ticket.ProjectionId);
            ViewData["ShoppingCartId"] = new SelectList(_context.ShoppingCarts, "Id", "Id", ticket.ShoppingCartId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var ticket = _ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _ticketService.GetTicket(id) != null;
        }

        // Администраторот потребно е да може да направи експортирање на сите билети во ексел датотека, според жанрот за филмот за кој се наменети.
        [Authorize(Roles = "Admin")]
        public IActionResult exportToExcel(string Genre)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Projections.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                        workbook.Worksheets.Add("Tickets");
                    worksheet.Cell(1, 1).Value = "Movie";
                    worksheet.Cell(1, 2).Value = "TimeOfProjection";
                    worksheet.Cell(1, 3).Value = "Quantity";
                    worksheet.Cell(1, 4).Value = "Price";
                    worksheet.Cell(1, 5).Value = "User";
                    var i = 2;
                    foreach (var item in _ticketService.GetAllTickets().Where(z => z.MovieGenre.Equals(Genre)))
                    {
                        worksheet.Cell(i, 1).Value = item.MovieName;
                        worksheet.Cell(i, 2).Value = item.TimeOfProjection;
                        worksheet.Cell(i, 3).Value = item.Quantity;
                        worksheet.Cell(i, 4).Value = item.Quantity * item.Price;
                        worksheet.Cell(i, 5).Value = item.UserName;
                        i++;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }

                }
            }
            catch (Exception ex) { }
            return View("Index");
            }
    }
}
