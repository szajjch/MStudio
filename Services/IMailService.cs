namespace barber_website.Services
{
	public interface IMailService
	{
		Task SendVerificationCode(string recipentEmail, string vericifactionCode);
		Task SendSuccessCode(string recipentEmail, DateTime date, string services);
	}
}
