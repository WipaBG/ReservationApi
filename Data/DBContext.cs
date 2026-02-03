using Microsoft.EntityFrameworkCore;
using ReservationApi.Domain;

namespace ReservationApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.Number)
            .IsUnique();

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Reservations)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}