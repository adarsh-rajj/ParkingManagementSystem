using BOL;
using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DAL.Context
{
    public class ParkingDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ParkingAllotment> ParkingAllotments { get; set; }
        public DbSet<VehicleRegistration> VehicleRegistrations { get; set; }
        public DbSet<ParkingBlock> ParkingBlocks { get; set; }

        public ParkingDbContext()
            : base("ParkingDbContext", throwIfV1Schema: false) { }

        public static ParkingDbContext Create()
        {
            return new ParkingDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleRegistration>()
                .HasKey(v => v.VehicleRcNoId); 

            modelBuilder.Entity<ParkingAllotment>()
                .HasKey(p => p.AllocationId);

            modelBuilder.Entity<ParkingBlock>()
                .HasKey(v => v.BlockId);

        }
    }
}

