using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Utility.DiConfig
{
    public static class DiAppSettingsConfig
    {
        public static IServiceCollection AddAppSettingsConfig(this IServiceCollection services,IConfiguration  configuration)
        {
            services.Configure<WazeForecastSettings>(configuration.GetSection("WazeForecast"));
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
            services.Configure<SendGridSettings>(configuration.GetSection("SendGrid"));
            services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
            return services;
        }
    }
}