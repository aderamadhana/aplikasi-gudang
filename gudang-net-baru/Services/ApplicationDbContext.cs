using gudang_net_baru.Models;
using gudang_net_baru.Models.Konfigurasi.Menu;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gudang_net_baru.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<MenuEntity> Menu { get; set; }
    }
}
