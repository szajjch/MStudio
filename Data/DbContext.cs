using barber_website.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_website.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }
        
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<OpeningHours> OpeningHours { get; set; }
        public DbSet<ConfirmationCode> ConfirmationCodes {  get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OtherDays> OtherDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpeningHours>()
                .HasKey(o => o.dayOfWeek);

			modelBuilder.Entity<ConfirmationCode>()
	            .HasKey(o => o.Email);

            modelBuilder.Entity<OtherDays>()
                .HasKey(o => o.date);
		}
    }
}
