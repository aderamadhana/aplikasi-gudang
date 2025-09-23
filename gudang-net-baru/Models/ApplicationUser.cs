using Microsoft.AspNetCore.Identity;

namespace gudang_net_baru.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
