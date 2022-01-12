using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore.Models
{
    public class CartItemModel
    {
        public int Id { get; set; }

        public string CartId { get; set; }
        
        public int BookId { get; set; }
    }
}
