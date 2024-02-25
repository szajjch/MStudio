namespace barber_website.Models
{
    public class ConfirmationCode
    {
		public int ConfirmationCodeId { get; set; }
		public string Email { get; set; }
		public string Code { get; set; }
		public DateTime CreatedAt { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ReservationType { get; set; }
		public DateTime ReservationDateTime { get; set; }
	}
}
