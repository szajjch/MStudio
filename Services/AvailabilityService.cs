using barber_website.Data;
using barber_website.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace barber_website.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ReservationDbContext _reservationDbContext;

        public AvailabilityService(ReservationDbContext reservationDbContext)
        {
            _reservationDbContext = reservationDbContext;
        }

        public async Task<List<DateTime>> GetAvailableSlots()
        {
			DateTime today = DateTime.Now.ToUniversalTime();
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

				DateTime currentTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.openHour.Hours, openingHour.openHour.Minutes, 0);
				DateTime closeTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, openingHour.closeHour.Hours, openingHour.closeHour.Minutes, 0);
				DateTime availableTime = currentTime;

				// Dodaj sloty co 30 minut
				while (availableTime  < closeTime)
				{
					availableSlots.Add(availableTime);
					availableTime  = availableTime.AddMinutes(30);
				}

				// Usuń zajęte sloty
				foreach (var reservation in reservations.Where(r => r.ReservationDateTime.Date == currentDate.Date))
				{
					var duration = reservation.Duration;
					var reservationStart = reservation.ReservationDateTime;
					var reservationEnd = reservation.ReservationDateTime.AddMinutes(duration);

					while (currentTime < reservationEnd) {
						if (currentTime >= reservationStart && currentTime < reservationEnd) 
						{
							availableSlots.Remove(currentTime);
						}
						currentTime  = currentTime.AddMinutes(30);
					}
				}
			}

			return availableSlots;
		}
	}
}
