namespace barber_website.Models
{
	public class OtherDays
	{
		public DateOnly date {  get; set; }
		public bool isOpen { get; set; }
		public TimeSpan openHour { get; set; }
		public TimeSpan closeHour { get; set; }
	}
}
