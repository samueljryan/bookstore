using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore.Models
{
    public class BookModel
    {
        public int Id { get; set; } 

        public string Title { get; set; }
        
        public string AuthorName { get; set; }
        
        public string Genre { get; set; }
       
        [Column (TypeName = "decimal(18,2")]
        public decimal Price { get; set; }
        
        public string Description { get; set; }
       
        public string ImageName { get; set; }
        
      





        public BookModel()
        {

        }
    }
}
