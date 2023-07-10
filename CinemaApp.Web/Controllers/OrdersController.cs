using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.DomainModels;
using CinemaApp.Repository;
using System.IO;
using GemBox.Document;
using System.Text;
using System.Security.Claims;
using CinemaApp.Service.Interface;

namespace CinemaApp.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _orderService = orderService;
        }

        // GET: Orders
        public IActionResult Index()
        {
            
            return View(_orderService.GetAllOrders());
        }


        // фактура во PDF датотека.
        public IActionResult GeneratePdf(Guid? id)
        {
            var order = _orderService.GetDetailsForOrder(id);
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template.docx");
            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{OrderNumber}}", id.ToString());
            document.Content.Replace("{{OrderDate}}", order.OrderPlaced.ToString());
            StringBuilder sb = new StringBuilder();
            foreach (var item in order.Tickets)
            {
                sb.AppendLine("Movie Title: " + item.MovieName + " Genre: " + item.MovieGenre + " Number of tickets: " + item.Quantity + " Total price:" + item.Quantity * item.Price + "\n");
            }
            document.Content.Replace("{{Tickets}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", order.TotalPrice.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "OrderInvoice.pdf");
        }

        public IActionResult MyOrders() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_orderService.GetAllOrdersByOwnerId(userId));
        }
    }
}
