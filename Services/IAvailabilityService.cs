namespace barber_website.Services
{
	public interface IAvailabilityService
	{
		Task<List<DateTime>> GetAvailableSlots();
	}
}
