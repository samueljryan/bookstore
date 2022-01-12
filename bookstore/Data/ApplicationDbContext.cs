using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using bookstore.Models;

namespace bookstore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<bookstore.Models.BookModel> BookModel { get; set; }
        public DbSet<bookstore.Models.CartItemModel> CartItemModel { get; set; }
        public DbSet<bookstore.Models.ImageModel> ImageModel { get; set; }
        public DbSet<bookstore.Models.ReviewModel> ReviewModel { get; set; }
        public DbSet<bookstore.Models.WishListItem> WishListItem { get; set; }
    }
}
