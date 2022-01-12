using bookstore.Data;
using bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace bookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<BookModel> Books = new List<BookModel>();
           
            var book1 = _context.BookModel.FirstOrDefault(m => m.Id == 19);
            var book2 = _context.BookModel.FirstOrDefault(m => m.Id == 27);
            var book3 = _context.BookModel.FirstOrDefault(m => m.Id == 8);
            var book4 = _context.BookModel.FirstOrDefault(m => m.Id == 1);

            Books.Add(book1);
            Books.Add(book2);
            Books.Add(book3);
            Books.Add(book4);
            
            ViewBag.OurPicks = Books;
            
            return View();
        }

  
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
