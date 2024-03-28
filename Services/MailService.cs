using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace barber_website.Services
{

	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;
		public MailService(IConfiguration configuration) {
			_configuration = configuration;
		}
		public async Task SendVerificationCode(string recipientEmail, string verificationCode)
		{
			var smtpServer = _configuration["EmailSettings:SmtpServer"];
			var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
			var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
			var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

			using (MimeMessage msg = new MimeMessage())
			{
				msg.From.Add(new MailboxAddress("M Studio", "mail@gmail.com"));
				msg.To.Add(new MailboxAddress(null, recipientEmail));
				msg.Subject = "Weryfikacja adresu e-mail";

				BodyBuilder bodyBuilder = new BodyBuilder();
				bodyBuilder.TextBody = $"Twój kod weryfikacyjny to: {verificationCode}";

				msg.Body = bodyBuilder.ToMessageBody();

				using (SmtpClient client = new SmtpClient())
				{
					await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
					await client.AuthenticateAsync(smtpUsername, smtpPassword);

					await client.SendAsync(msg);
					await client.DisconnectAsync(true);
				}
			}
		}
	}
}
