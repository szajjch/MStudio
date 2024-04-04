using barber_website.Data;
using barber_website.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_website.Services
{
	public class InitService : IInitService
	{
		private readonly ReservationDbContext _reservationDbContext;

		public InitService(ReservationDbContext reservationDbContext)
		{
			_reservationDbContext = reservationDbContext;
		}

		public async Task<List<Offer>> GetOffers()
		{
			return await _reservationDbContext.Offers.ToListAsync();
		}
	}
}
