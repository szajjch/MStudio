using barber_website.Data;
using barber_website.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace barber_website.Services
{
    public class Availability
    {
        private readonly ReservationDbContext _reservationDbContext;

        public Availability(ReservationDbContext reservationDbContext)
        {
            _reservationDbContext = reservationDbContext;
        }

        public async Task<List<DateTime>> GetAvailableSlots()
        {
			DateTime today = DateTime.Today;
			today = today.ToUniversalTime();
			DateTime weekLater = today.AddDays(7);

            // Pobierz zarezerwowane terminy 7 dni do przodu
			var reservations = await _reservationDbContext.Reservations
	            .Where(r => r.ReservationDateTime >= today && r.ReservationDateTime <= weekLater)
	            .ToListAsync();

            // Pobierz godziny otwarcia i zamkięcia salonu
            var openingHours = await _reservationDbContext.OpeningHours.ToListAsync();

            // Inicjalizacja listy dostępnych slotów
            List<DateTime> availableSlots = new List<DateTime>();

            foreach (var day in Enumerable.Range(0, 7))
            {
                var currentDate = today.AddDays(day);

				var openingHour = openingHours.FirstOrDefault(sh => sh.dayOfWeek == currentDate.DayOfWeek.ToString());
				if (openingHour == null || !openingHour.isOpen)
					continue;

				var curTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.openHour.Hours, openingHour.openHour.Minutes, 0);
				var closeTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.closeHour.Hours, openingHour.closeHour.Minutes, 0);

				foreach (var reservation in reservations.Where(r => r.ReservationDateTime.Date == currentDate.Date))
				{
					var duration = reservation.Duration;
					var reservationEnd = reservation.ReservationDateTime.AddMinutes(duration);

					while (curTime < reservationEnd) {
						availableSlots.Remove(curTime);
						curTime = curTime.AddMinutes(15);
					}
				}

				while (curTime < closeTime)
				{
					availableSlots.Add(curTime);
					curTime = curTime.AddMinutes(15);
				}
			}

			return availableSlots;
		}
	}
}
