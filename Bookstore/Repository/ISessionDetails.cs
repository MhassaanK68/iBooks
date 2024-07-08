namespace Bookstore.Repository
{
    public interface ISessionDetails
    {
        bool IsLogin();
        bool IsAdmin();
        bool GetAllDetails();
    }
}
