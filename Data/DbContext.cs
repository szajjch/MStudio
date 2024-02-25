using barber_website.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_website.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }
        
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<OpeningHours> OpeningHours { get; set; }


        // Dodać confirmation code
    }
}
