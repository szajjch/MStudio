using barber_website.Models;

namespace barber_website.Services
{
	public interface IInitService
	{
		Task<List<Offer>> GetOffers();
		Task<List<OpeningHours>> GetCalendar();
	}
}
