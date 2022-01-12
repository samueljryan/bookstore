using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bookstore.Data;
using bookstore.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace bookstore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Review
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReviewModel.ToListAsync());
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewModel = await _context.ReviewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviewModel == null)
            {
                return NotFound();
            }

            return View(reviewModel);
        }

        // GET: Review/Create
        public IActionResult Create(int bookId)
        {
            var newReview = new ReviewModel();
            newReview.ItemId = bookId;
            return PartialView("~/Views/Review/_AddReview.cshtml", newReview);
        }

        // POST: Review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,UserId,Name,Created,Rating,Review")] ReviewModel reviewModel)
        {
            if (ModelState.IsValid)
            {
                var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                reviewModel.UserId = user;
                _context.Add(reviewModel);
                await _context.SaveChangesAsync();
                int bookId = reviewModel.ItemId;
                List<ReviewModel> revlist = _context.ReviewModel.ToList();
                var result = from r in revlist
                             where r.ItemId == bookId
                             select r;
                ViewBag.reviews = result;
                
                return RedirectToAction("Details", "Book", new { id = bookId  });
            }
            return PartialView("~/Views/Book/_AddReview.cshtml", reviewModel);
        }

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewModel = await _context.ReviewModel.FindAsync(id);
            if (reviewModel == null)
            {
                return NotFound();
            }
            return View(reviewModel);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,UserId,Name,Created,Rating,Review")] ReviewModel reviewModel)
        {
            if (id != reviewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewModelExists(reviewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Book", new { id = reviewModel.ItemId });
            }
            return View(reviewModel);
        }

        // GET: Review/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewModel = await _context.ReviewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviewModel == null)
            {
                return NotFound();
            }

            return View(reviewModel);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviewModel = await _context.ReviewModel.FindAsync(id);
            _context.ReviewModel.Remove(reviewModel);
            await _context.SaveChangesAsync();
            List<ReviewModel> revlist = _context.ReviewModel.ToList();
            var result = from r in revlist
                         where r.ItemId == id
                         select r;
            ViewBag.reviews = result;
            return RedirectToAction("Details", "Book", new { id = reviewModel.ItemId });
        }

        private bool ReviewModelExists(int id)
        {
            return _context.ReviewModel.Any(e => e.Id == id);
        }
    }
}
