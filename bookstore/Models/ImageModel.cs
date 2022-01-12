using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookstore.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageId { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }
        [NotMapped]
        
        public IFormFile File { get; set; }
       
        public ImageModel() 
        {
        }
    }
}
