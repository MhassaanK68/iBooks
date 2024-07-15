using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class BooksInputModel
    {

        public string BookName { get; set; }
        
        public string Author { get; set; }
        
        public string? ImageSource { get; set; }

        public string Category { get; set; }

        public string? Status { get; set; }

        public IFormFile CoverPhoto { get; set; }

    }



        
}
