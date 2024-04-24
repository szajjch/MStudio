using barber_website.Models;

namespace barber_website.Services
{
	public interface IBookingService
	{
		Task<bool> IsReservationAvailable(DateTime resDateTime, int duration, string email);
		Task BookHour(Reservation res);
		Task StartVerification(string email);
		Task<bool> VerifyCode(string code, string email);
		string GenerateCode(int length);
	}
}
