using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class AdminActivityModel
    {
        [Key]
        public int ActivityID { get; set; } 
        public string AdminName { get; set; }
        public AdminActivityType Activity { get; set; }
        public string MetaData { get; set; }
        public string ActivityMessage { get; set; }
        public DateTime Time { get; set; }

    }

    public enum AdminActivityType
    {
        CreateUser,
        DeleteUser,
        EditUser,
        CreateBook,
        DeleteBook,
        PanelLogin
    }


}
