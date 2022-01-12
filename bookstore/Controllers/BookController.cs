using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bookstore.Data;
using bookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookModel.ToListAsync());
        }

        public async  Task<IActionResult> ClassicsIndex()
        {
            var classic = from b in _context.BookModel
                          where b.Genre == "Classics"
                          select b;
            return View(await classic.ToListAsync());
        }

        public async Task<IActionResult> FantasyIndex()
        {  
            var fantasy = from b in _context.BookModel
                           where b.Genre == "Fantasy"
                           select b;
            return View(await fantasy.ToListAsync());
        }

        public async Task<IActionResult> SciFiIndex()
        {
            var scifi = from b in _context.BookModel
                          where b.Genre == "Science Fiction"
                          select b;
            return View(await scifi.ToListAsync());
        }

        public async Task<IActionResult> HistoryIndex()
        {
            var history = from b in _context.BookModel
                        where b.Genre == "History"
                        select b;
            return View(await history.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            ViewBag.picName = bookModel.ImageName;

            List<ReviewModel> revlist = _context.ReviewModel.ToList();
            var result = (from r in revlist
                          where r.ItemId == id
                          select r);
            
            ViewBag.reviews = result;
            
            ViewBag.totalRatings = result.Count();

            if (result.Count() == 0){
                ViewBag.avgRating = 0;
                ViewBag.percentOfFive = 0;
                ViewBag.percentOfFour = 0;
                ViewBag.percentOfThree = 0;
                ViewBag.percentOfTwo = 0 ;
                ViewBag.percentOfOne = 0;
            }

            else
            {
                var avg = (from r in result
                           select r.Rating).Average();
                ViewBag.avgRating = Math.Round(avg);

                ViewBag.percentOfFive = (((from r in result
                                           where r.Rating == 5
                                           select r).Count()) / result.Count()) * 100;

                ViewBag.percentOfFour = (((from r in result
                                           where r.Rating == 4
                                           select r).Count()) / result.Count()) * 100;

                ViewBag.percentOfThree = (((from r in result
                                            where r.Rating == 3
                                            select r).Count()) / result.Count()) * 100;

                ViewBag.percentOfTwo = (((from r in result
                                          where r.Rating == 2
                                          select r).Count()) / result.Count()) * 100;


                ViewBag.percentOfOne = (((from r in result
                                          where r.Rating == 1
                                          select r).Count()) / result.Count()) * 100;

            }
            
            return View(bookModel);
        }

        // GET: Book/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {

            return View();


        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,AuthorName,Genre,Price,Description,ImageName")] BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookModel);
        }

        // GET: Book/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            return View(bookModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorName,Genre,Price,Description,ImageName")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!bookModelExists(bookModel.Id))
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
            return View(bookModel);
        }

        // GET: Book/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: Book/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookModel = await _context.BookModel.FindAsync(id);
            _context.BookModel.Remove(bookModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool bookModelExists(int id)
        {
            return _context.BookModel.Any(e => e.Id == id);
        }


    }
    
}
