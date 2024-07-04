using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition.Convention;

namespace Bookstore.Models
{
    public class Users
    {
        [Column]
        public string Email { get; set; }

        [Key]
        public string UserName { get; set; }
        
        [Column]
        public string Password { get; set; }


    }

}
