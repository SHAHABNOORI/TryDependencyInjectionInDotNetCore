using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WazeCredit.Models;
using WazeCredit.Service;
using WazeCredit.Service.LifeTimeExample;

namespace WazeCredit.Utility.DiConfig
{
    public static class ConfigureDiServices
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services){
            services.AddTransient<IMarketForecaster, MarketForecasterV2>();

            //Only work on singleton
            //services.AddSingleton<IMarketForecaster>(new MarketForecaster());

            //services.Configure<WazeForecastSettings>(Configuration.GetSection("WazeForecast"));
            //services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            //services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));
            //services.Configure<TwilioSettings>(Configuration.GetSection("Twilio"));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();


            //services.AddScoped<IValidationChecker, AddressValidationChecker>();
            //services.AddScoped<IValidationChecker, CreditValidationChecker>();
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker,AddressValidationChecker>());
            //services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());

            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Scoped<IValidationChecker,AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>()
            });

            services.AddScoped<CreditApprovedHigh>();
            services.AddScoped<CreditApprovedLow>();


            services.AddScoped<Func<CreditApproveType, ICreditApproved>>(sp => range =>
            {
                return range switch
                {
                    CreditApproveType.High => sp.GetService<CreditApprovedHigh>(),
                    CreditApproveType.Low => sp.GetService<CreditApprovedLow>(),
                    _ => sp.GetService<CreditApprovedLow>()
                };
            });

            services.AddScoped<ICreditValidator, CreditValidator>();


            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();
            //services.TryAddTransient<IMarketForecaster, MarketForecaster>();
            //services.Replace(ServiceDescriptor.Transient<IMarketForecaster, MarketForecaster>());
            //services.RemoveAll<IMarketForecaster>();
            return services;
        }
    }
}