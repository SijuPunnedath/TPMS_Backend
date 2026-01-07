

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using TPMS.Infrastructure.POCO;
using TPMS.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public SmtpEmailService(IOptions<SmtpSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtp = new SmtpClient(_settings.Host)
        {
            Port = _settings.Port,
            Credentials = new NetworkCredential(_settings.User, _settings.Password),
            EnableSsl = true
        };

        var msg = new MailMessage(_settings.From, to, subject, body);
        await smtp.SendMailAsync(msg);
    }
}




/*
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TPMS.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;
    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    } 
    
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtp = new SmtpClient(_config["Smtp:Host"])
        {
            Port = int.Parse(_config["Smtp:Port"]),
            Credentials = new NetworkCredential(
                _config["Smtp:User"],
                _config["Smtp:Password"]
            ),
            EnableSsl = true
        };

        var msg = new MailMessage(_config["Smtp:From"], to, subject, body)
        {
            IsBodyHtml = false
        };

        await smtp.SendMailAsync(msg);
    }
}
*/