namespace Bookstore.Models
{
    public class BooksViewModel
    {
        public IQueryable<BooksModel> BookCategory { get; set; }
        public string CategoryName { get; set; }


    }
}
