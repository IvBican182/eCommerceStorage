

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StorageApi.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}