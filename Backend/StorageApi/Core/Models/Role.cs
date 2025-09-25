


using Microsoft.AspNetCore.Identity;

namespace StorageApi.Core.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}