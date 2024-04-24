using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json.Linq;

namespace barber_website.Services
{

	public class MailService : IMailService
	{
		private readonly SmtpService _smtpService;

		public MailService(SmtpService smtpService)
		{
			_smtpService = smtpService;
		}

		public async Task SendVerificationCode(string recipientEmail, string verificationCode)
		{
			await _smtpService.SendEmailAsync(recipientEmail, "Weryfikacja adresu e-mail", $"Twój kod weryfikacyjny to: {verificationCode}");
		}

		public async Task SendSuccessCode(string recipientEmail, DateTime date, string services)
		{
			JArray servicesArray = JArray.Parse(services);
			string selectedServices = "";

			foreach (var service in servicesArray)
			{
				foreach (var property in service.Children<JProperty>())
				{
					if (property.Value.Value<int>() > 0)
						selectedServices += $"{property.Name}: {property.Value}\n";
				}
			}

			string body = @$"
				Cześć, informujemy o potwierdzeniu twojej rezerwacji.

				{date}
				{selectedServices}	
			";

			await _smtpService.SendEmailAsync(recipientEmail, "Potwierdzenie rezerwacji", body);
		}
	}
}
