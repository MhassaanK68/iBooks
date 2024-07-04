using Bookstore.Models;

namespace Bookstore.Repository
{
    public class BookRepository : IBooks
    {
        public List<Book> GetAllBooks()
        {
            return DataSource();
        }

        public Book GetBookByID(int ID)
        {
            return DataSource().Where(x => x.BookID == ID).FirstOrDefault();
        }

        public List<Book> GetFeaturedProducts()
        {
            return FeaturedProducts();
        }

        // Demo of SQL
        private List<Book> DataSource()
        {
            var Data =  new List<Book>
            {
                new Book {BookName = "Harry Potter Series", Author = "JK Rowling", Price = 2400, ImageSource="/Assets/Images/Book-Thumnails/HarryPotter.jpg", BookID=0},
                new Book {BookName = "Rich Dad Poor Dad", Author = "Robert Kiyosaki", Price = 1200, ImageSource="https://placehold.co/300x150", BookID = 1},
                new Book {BookName = "Diary Of A Wimpy Kid", Author = "Jeff Kinney", Price = 600, ImageSource = "https://placehold.co/300x150", BookID = 2},
                new Book {BookName = "To Kill a Mockingbird", Author = "Harper Lee", Price = 1800, ImageSource = "https://placehold.co/300x150", BookID = 3},
                new Book {BookName = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 2000, ImageSource = "https://placehold.co/300x150", BookID = 4},
                new Book {BookName = "1984", Author = "George Orwell", Price = 150 , ImageSource =  "https://placehold.co/300x150", BookID = 5},
                new Book {BookName = "The Catcher in the Rye", Author = "J.D. Salinger", Price = 1700, ImageSource = "https://placehold.co/300x150", BookID = 6},
                new Book {BookName = "Pride and Prejudice", Author = "Jane Austen", Price = 1600, ImageSource = "https://placehold.co/300x150", BookID = 7}
            };
            return Data;
    
        }

        private List<Book> FeaturedProducts()
        {
            var Featured = new List<Book>
            {
                GetBookByID(0),
                GetBookByID(7),
                GetBookByID(2),
                GetBookByID(4)

            };
            return Featured;
        }
    }
}
