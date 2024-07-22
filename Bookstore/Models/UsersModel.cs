using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition.Convention;

namespace Bookstore.Models
{
    public class UsersModel
    {
        [Column]
        public string Email { get; set; }

        [Key]
        public string Username { get; set; }
        
        [Column]
        public string Password { get; set; }

        [Column]
        public RoleType role { get; set; }

        [Column]
        public string? PhoneNumber { get; set; }

    }

        public enum RoleType
        {
            User,
            Admin
        }

}
