using bookstore.Data;
using bookstore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace bookstore.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PaymentsController(ApplicationDbContext context)
        {
            StripeConfiguration.ApiKey = "sk_test_51KBt7BGC5XCrczcVOpot5BnFUd60Xt8QmswXHev70rYqCvuL0V7WdkXvCP69tXcpeWeAVXe7WHwzyzclRtZKu7Cz00unlRVz9f";
            _context = context;
        }


        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            var cartid = HttpContext.Session.GetString("CartId");
            var bookIds = (from c in _context.CartItemModel
                           where c.CartId == cartid
                           select c.BookId);
            List<BookModel> BooksinCart = new List<BookModel>();

            foreach (var id in bookIds)
            {
                var book = _context.BookModel.Find(id);
                BooksinCart.Add(book);
            }
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
      
                Mode = "payment",
                SuccessUrl = "https://localhost:44305/Payments/Success",
                CancelUrl = "https://localhost:44305/Cart/ViewCart"

            };
            
            
            
            foreach (var book in BooksinCart)
            {
                var item = new SessionLineItemOptions
                {
                    
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(book.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = book.Title,
                        },
                    },
                    Quantity = 1,
                    
                };
                options.LineItems.Add(item);
            }
            

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            
        }


        public IActionResult Success()
        {
            return View();
        }

   
    }
}
