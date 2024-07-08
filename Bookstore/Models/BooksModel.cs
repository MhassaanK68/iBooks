using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class BooksModel
    {
        [Key]
        public int BookID { get; set; }
        
        [Column("Name", TypeName = "varchar(100)")]
        public string BookName { get; set; }
        
        public string Author { get; set; }
        
        public string ImageSource { get; set; }

        public Categories Category { get; set; }


    }

    public enum Categories 
    {
        Sci_Fi,
        Adventure,
        Romantic,
        Horror,
        Humor,
        Fantasy,
        Thrillers

    }

        
}
