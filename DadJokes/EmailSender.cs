using Microsoft.AspNetCore.Identity.UI.Services;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {

        var smtpClient = new SmtpClient("live.smtp.mailtrap.io", 587)
        {
            Credentials = new NetworkCredential("api", ""),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress("hello@demomailtrap.com"),
            Subject = $"{email} wants an account confirmed on Riddle World",
            Body = htmlMessage,
            IsBodyHtml = true
        };

        message.To.Add("abel97.ag@gmail.com");

        await smtpClient.SendMailAsync(message);


    }
}