using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore.Models
{
    public class WishListItem
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int BookId { get; set; }
    }
}
