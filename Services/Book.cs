using barber_website.Data;
using barber_website.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_website.Services
{
	public class Book
	{
		private readonly ReservationDbContext _reservationDbContext;

		public Book(ReservationDbContext reservationDbContext)
		{
			_reservationDbContext = reservationDbContext;
		}

		public async Task<bool> IsSlotAvailable(DateTime resDateTime, int duration)
		{
			var existingReservation = await _reservationDbContext.Reservations
				.Where(r => r.ReservationDateTime <= resDateTime && r.ReservationDateTime.AddMinutes(r.Duration) >= resDateTime)
				.FirstOrDefaultAsync();

			return existingReservation == null;
		}

		public async Task BookHour(Reservation res)
		{




			_reservationDbContext.Add(res);
			await _reservationDbContext.SaveChangesAsync();
		}
	}
}
