using FuelBe.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FuelBe.Database {
    public class ReservationDbContext : DbContext {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

        public virtual DbSet<Models.User> Users => Set<Models.User>();
        public virtual DbSet<Models.Vehicle> Vehicles => Set<Models.Vehicle>();
        public virtual DbSet<Models.Reservation> Reservations => Set<Models.Reservation>();
        public virtual DbSet<Models.Refueling> Refuelings => Set<Models.Refueling>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
            //---------------------------------------------------------------------
            //User
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Reservations)
                .WithOne(x => x.User)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>()
              .HasMany(x => x.Refuelings)
              .WithOne(x => x.User)
              .HasPrincipalKey(x => x.Id)
              .HasForeignKey(x => x.UserId);
            //---------------------------------------------------------------------
            //Vehicle
            modelBuilder.Entity<Vehicle>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Vehicle>()
                .HasMany(x => x.Reservations)
                .WithOne(x => x.Vehicle)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.VehicleId);
            modelBuilder.Entity<Vehicle>()
                .HasMany(x => x.Refuelings)
                .WithOne(x => x.Vehicle)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.VehicleId);
            //----------------------------------------------------------------------
            //Reservation
            modelBuilder.Entity<Reservation>()
                .HasKey(x => x.Id);
            //--------------------------------------------------------------------------
            modelBuilder.Entity<Refueling>()
                .HasKey(x => x.Id);
        }

    }
}
