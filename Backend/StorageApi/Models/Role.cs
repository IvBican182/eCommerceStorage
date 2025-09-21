

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StorageApi.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}