using Bookstore.Models;

namespace Bookstore.Repository
{
    public interface IBooks
    {
        List<Book> GetAllBooks();
        Book GetBookByID(int name);
        List<Book> GetFeaturedProducts();


    }
}
