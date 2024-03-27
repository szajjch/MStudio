namespace barber_website.Models
{
    public class ConfirmationCode
    {
		public string Email { get; set; }
		public string Code { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
