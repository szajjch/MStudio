using barber_website.Components;
using barber_website.Data;
using barber_website.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddDbContext<ReservationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IInitService, InitService>();

builder.Services.AddSingleton<SmtpService>(provider =>
{
	var smtpServer = configuration["EmailSettings:SmtpServer"];
	var smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
	var smtpUsername = configuration["EmailSettings:SmtpUsername"];
	var smtpPassword = configuration["EmailSettings:SmtpPassword"];

	return new SmtpService(smtpServer, smtpPort, smtpUsername, smtpPassword);
});

builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
