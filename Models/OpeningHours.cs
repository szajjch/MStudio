namespace barber_website.Models
{
    public class OpeningHours
    {
        public int id {  get; set; }
        public DayOfWeek dayOfWeek { get; set; }
        public bool isOpen { get; set; } 
        public TimeSpan openHour { get; set; }
        public TimeSpan closeHour { get; set; }
    }
}
