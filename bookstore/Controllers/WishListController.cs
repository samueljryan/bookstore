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
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WishList
        public async Task<IActionResult> Index()
        {
            return View(await _context.WishListItem.ToListAsync());
        }

        // GET: WishList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishListItem = await _context.WishListItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishListItem == null)
            {
                return NotFound();
            }

            return View(wishListItem);
        }

        // GET: WishList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WishList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,BookId")] WishListItem wishListItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishListItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishListItem);
        }

        // GET: WishList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishListItem = await _context.WishListItem.FindAsync(id);
            if (wishListItem == null)
            {
                return NotFound();
            }
            return View(wishListItem);
        }

        // POST: WishList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BookId")] WishListItem wishListItem)
        {
            if (id != wishListItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListItemExists(wishListItem.Id))
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
            return View(wishListItem);
        }

        // GET: WishList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishListItem = await _context.WishListItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishListItem == null)
            {
                return NotFound();
            }

            return View(wishListItem);
        }

        // POST: WishList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishListItem = await _context.WishListItem.FindAsync(id);
            _context.WishListItem.Remove(wishListItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListItemExists(int id)
        {
            return _context.WishListItem.Any(e => e.Id == id);
        }




        [Authorize]
        public async Task<IActionResult> AddtoWishlist(int Id)
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            WishListItem item = new WishListItem() {
                BookId = Id,
                UserId = user    
            };
            _context.WishListItem.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Book", new {id = Id });

        }

        [Authorize]
        public async Task<IActionResult> RemoveFromWishlist(int Id)
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = (from w in _context.WishListItem
                        where w.UserId == user && w.BookId == Id
                        select w).FirstOrDefault();
            _context.WishListItem.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewWishlist", "WishList");

        }



      
        public async Task<IActionResult> ViewWishlist()
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookIds = (from w in _context.WishListItem
                           where w.UserId == user
                           select w.BookId);

            List<BookModel> BooksinCart = new List<BookModel>();

            foreach (var id in bookIds)
            {
                var book = await _context.BookModel.FindAsync(id);
                BooksinCart.Add(book);
            }

            return View("~/Views/WishList/Index.cshtml", BooksinCart);

        }




        [Authorize]
        public async Task<IActionResult> ClearWishlist()
        {
            var user = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = (from w in _context.WishListItem
                         where w.UserId == user
                         select w);

            foreach ( var item in wishlist)
            {
                _context.WishListItem.Remove(item);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewWishlist", "WishList");



        }
    }
}
