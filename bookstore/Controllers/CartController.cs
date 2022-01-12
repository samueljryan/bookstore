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
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            return View(await _context.CartItemModel.ToListAsync());
        }

      

        private string GetCartId()
        {
            if (HttpContext.Session.GetString("CartId") == null)
            {
                Guid tempCartId = Guid.NewGuid();
                HttpContext.Session.SetString("CartId", tempCartId.ToString());
            }
            return HttpContext.Session.GetString("CartId");
        }

        //POST
        public async Task <IActionResult> AddItemtoShoppingCart(int Id)
        {
            var cartid = GetCartId();
         
            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };
           
            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Book");
        }


        public async Task<IActionResult> AddtoCartHome(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AddtoCartClassic(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("ClassicsIndex", "Book");
        }

        public async Task<IActionResult> AddtoCartFantasy(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("FantasyIndex", "Book");
        }

        public async Task<IActionResult> AddtoCartSciFi(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("SciFiIndex", "Book");
        }

        public async Task<IActionResult> AddtoCartHistory(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("HistoryIndex", "Book");
        }


        public async Task<IActionResult> AddtoCartDetails(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Book", new {id= Id});
        }



        public async Task<IActionResult> AddtoCartFromWish(int Id)
        {
            var cartid = GetCartId();

            CartItemModel newCartItem = new CartItemModel()
            {
                CartId = cartid,
                BookId = Id
            };

            _context.CartItemModel.Add(newCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewWishlist", "WishList");
        }




        public async Task<IActionResult> RemoveItemfromShoppingCart(int Id)
        {
            var cartid = GetCartId();
            var item =  (from c in _context.CartItemModel
                              where c.CartId == cartid && c.BookId == Id
                              select c).SingleOrDefault();

            _context.CartItemModel.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewCart", "Cart");

        }
    

     

        public  async Task<IActionResult> ClearCart()
        {
            var cartid = GetCartId();
            var cart = (from c in _context.CartItemModel
                        where c.CartId == cartid
                        select c);
            
            foreach (var item in cart)
            {
                 _context.CartItemModel.Remove(item);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCart", "Cart");
        }

        public async Task<IActionResult> ViewCart()
        {
            var cartid = GetCartId();

            var bookIds = (from c in _context.CartItemModel
                        where c.CartId == cartid
                        select c.BookId);

            List<BookModel> BooksinCart = new List<BookModel>();

            foreach (var id in bookIds)
            {
                var book = await _context.BookModel.FindAsync(id);
                BooksinCart.Add(book);
            }

            ViewBag.total = (from p in BooksinCart
                             select p.Price).Sum();

            return View("~/Views/Cart/Index.cshtml", BooksinCart);

        }

        
    }
}
