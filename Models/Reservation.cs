namespace barber_website.Models
{
    public class Reservation
    {
		public int id {  get; set; }
		public int ReservationId { get; set; }
		public string FirstName { get; set; }
		public int Age { get; set; }
		public string Email { get; set; }
		public string ReservationType { get; set; }
		public int Duration { get; set; }
		public DateTime ReservationDateTime { get; set; }
	}
}
