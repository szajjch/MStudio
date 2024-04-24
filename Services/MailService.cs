using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
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
			if (string.IsNullOrEmpty(services))
			{
				return;
			}

			JObject servicesObject;
			try
			{
				servicesObject = JObject.Parse(services);
			}
			catch (JsonReaderException ex)
			{
				Console.WriteLine($"Błąd podczas parsowania danych JSON: {ex.Message}");
				return;
			}

			string selectedServices = "";

			foreach (var service in servicesObject.Properties())
			{
				if (service.Value.Value<int>() > 0)
				{
					selectedServices += $"{service.Name}: {service.Value}\n";
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