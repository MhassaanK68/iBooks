using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class Book 
    {
        [Key]
        public int BookID { get; set; }
        
        [Column("Name", TypeName = "varchar(100)")]
        public string BookName { get; set; }

        [Column("Price", TypeName = "int")]
        public int Price { get; set; }
        
        public string Author { get; set; }
        
        public string ImageSource { get; set; }



        

    }
}
