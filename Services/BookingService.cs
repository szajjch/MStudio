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

		public async Task<bool> IsReservationAvailable(DateTime resDateTime, int duration, string email)
		{
			resDateTime = resDateTime.ToUniversalTime();
			var existingReservation = await _reservationDbContext.Reservations
				.AnyAsync(r => r.ReservationDateTime <= resDateTime && r.ReservationDateTime.AddMinutes(duration) >= resDateTime);

			var reservationByEmail = await _reservationDbContext.Reservations
				.AnyAsync(r => r.Email == email);

			return !existingReservation && !reservationByEmail;
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
