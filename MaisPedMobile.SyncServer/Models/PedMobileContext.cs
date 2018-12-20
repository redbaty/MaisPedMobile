using Microsoft.EntityFrameworkCore;

namespace MaisPedMobile.SyncServer.Models
{
    public class PedMobileContext : DbContext
    {
        public DbSet<Enterprise> Enterprises { get; set; }

        public DbSet<Salesman> Salesman { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public PedMobileContext(DbContextOptions options) : base(options)
        {
        }
    }
}