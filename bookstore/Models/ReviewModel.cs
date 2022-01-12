using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
       
        public int  ItemId { get; set; }
        
        public string UserId { get; set; }
        
        public string Name { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now;
        
        public int Rating { get; set; }
        
        public string Review { get; set; }
        
        public ReviewModel()
        {

        }
    }
}
