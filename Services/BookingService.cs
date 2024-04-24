using barber_website.Data;
using barber_website.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace barber_website.Services
{
	public class BookingService : IBookingService
	{
		private readonly ReservationDbContext _reservationDbContext;
		private readonly IMailService _mailService;

		public BookingService(ReservationDbContext reservationDbContext, IMailService mailService)
		{
			_reservationDbContext = reservationDbContext;
			_mailService = mailService;
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

			await _mailService.SendSuccessCode(res.Email, res.ReservationDateTime, res.Services);
		}

		public async Task StartVerification(string email)
		{
			var oldCode = await _reservationDbContext.ConfirmationCodes
				.FirstOrDefaultAsync(c => c.Email == email);

			DateTime createdAt = DateTime.UtcNow;
			if (oldCode != null)
			{
				TimeSpan difference = createdAt - oldCode.CreatedAt.ToUniversalTime();

				if (difference.TotalHours > 1)
				{
					_reservationDbContext.ConfirmationCodes.Remove(oldCode); // usun stary

					string verCode = GenerateCode(6);

					ConfirmationCode ver = new ConfirmationCode
					{
						Email = email,
						Code = verCode,
						CreatedAt = createdAt
					};

					_reservationDbContext.ConfirmationCodes.Add(ver); // dodaj nowy
					await _reservationDbContext.SaveChangesAsync();

					await _mailService.SendVerificationCode(email, verCode);
				}
				else
				{
					await _mailService.SendVerificationCode(email, oldCode.Code);
				}
			}
			else
			{
				string verCode = GenerateCode(6);

				ConfirmationCode ver = new ConfirmationCode
				{
					Email = email,
					Code = verCode,
					CreatedAt = createdAt
				};

				_reservationDbContext.ConfirmationCodes.Add(ver);
				await _reservationDbContext.SaveChangesAsync();

				await _mailService.SendVerificationCode(email, verCode);
			}
		}

		public async Task<bool> VerifyCode(string code, string email)
		{
			var confirmationCode = await _reservationDbContext.ConfirmationCodes.FirstOrDefaultAsync(c => c.Email == email);
			if (confirmationCode != null)
			{
				if (string.Equals(code, confirmationCode.Code, StringComparison.OrdinalIgnoreCase))
				{
					_reservationDbContext.ConfirmationCodes.Remove(confirmationCode);
					await _reservationDbContext.SaveChangesAsync();

					return true;
				}
			}
			return false;
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
