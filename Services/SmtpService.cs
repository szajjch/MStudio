using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace barber_website.Services
{
	public class SmtpService : IDisposable
	{
		private readonly SmtpClient _client;
		private Timer _keepAliveTimer;

		public SmtpService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword) 
		{
			_client = new SmtpClient();

			_client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
			_client.Authenticate(smtpUsername, smtpPassword);

			_keepAliveTimer = new Timer(async _ => await KeepSmtpConnectionAliveAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
		}
		public SmtpClient Client => _client;

		public async Task SendEmailAsync(string recipentEmail, string subject, string body)
		{
			MimeMessage msg = new MimeMessage();
			msg.From.Add(new MailboxAddress("MStudio", "test@gmail.com"));
			msg.To.Add(new MailboxAddress(null, recipentEmail));
			msg.Subject = subject;

			BodyBuilder bodyBuilder = new BodyBuilder();
			bodyBuilder.TextBody = body;
			msg.Body = bodyBuilder.ToMessageBody();

			await _client.SendAsync(msg);
		}

		private async Task KeepSmtpConnectionAliveAsync()
		{
			try
			{
				await _client.NoOpAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Błąd podczas utrzymywania aktywnego połączenia SMTP: {ex.Message}");
			}
		}

		public void Dispose()
		{
			_keepAliveTimer.Dispose();
			_client.Disconnect(true);
			_client.Dispose();
		}
	}
}
