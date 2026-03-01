


using Microsoft.Extensions.Options;
using TPMS.Infrastructure.POCO;
using TPMS.Infrastructure.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class TwilioSmsService : ISmsService
{
    private readonly TwilioSettings _settings;

    public TwilioSmsService(IOptions<TwilioSettings> settings)
    {
        _settings = settings.Value;
        TwilioClient.Init(_settings.Sid, _settings.Token);
    }
 
    public async Task SendSmsAsync(string phone, string message)
    {
        await MessageResource.CreateAsync(
            body: message,
            from: new Twilio.Types.PhoneNumber(_settings.From),
            to: new Twilio.Types.PhoneNumber(phone)
        );
    }
}



/*
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TPMS.Infrastructure.Services;

public class TwilioSmsService : ISmsService
{
    private readonly IConfiguration _config;
    public TwilioSmsService(IConfiguration config)
    {
        _config = config;
        TwilioClient.Init(_config["Twilio:Sid"], _config["Twilio:Token"]);
    }

    public async Task SendSmsAsync(string phone, string message)
    {
        await MessageResource.CreateAsync(
            body: message,
            from: new Twilio.Types.PhoneNumber(_config["Twilio:From"]),
            to: new Twilio.Types.PhoneNumber(phone)
        );
    } 
}*/