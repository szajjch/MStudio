namespace barber_website.Services
{
	public interface IMailService
	{
		Task SendVerificationCode(string recipentEmail, string vericifactionCode);
	}
}
