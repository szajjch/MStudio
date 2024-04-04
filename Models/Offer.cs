namespace barber_website.Models
{
	public class Offer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Duration { get; set; }
		public decimal Price { get; set; }
	}
}
