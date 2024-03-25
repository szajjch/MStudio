using barber_website.Data;
using barber_website.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
			resDateTime = resDateTime.ToUniversalTime();
			var existingReservation = await _reservationDbContext.Reservations
				.Where(r => r.ReservationDateTime <= resDateTime && r.ReservationDateTime.AddMinutes(duration) >= resDateTime)
				.FirstOrDefaultAsync();

			return existingReservation == null;
		}

		public async Task BookHour(Reservation res)
		{




			_reservationDbContext.Add(res);
			await _reservationDbContext.SaveChangesAsync();
		}

		public string GenerateCode(int lenght)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i<lenght; i++)
			{
				builder.Append(chars[random.Next(chars.Length)]);
			}
			return builder.ToString();
		}
	}
}
