using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WazeCredit.Data;
using WazeCredit.Data.Repository;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMarketForecaster _marketForecaster;
        private readonly ICreditValidator _creditValidator;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly StripeSettings _stripeSettings;
        //private readonly SendGridSettings _sendGridSettings;
        //private readonly TwilioSettings _twilioSettings;
        private readonly WazeForecastSettings _wazeForecastSettings;
        private readonly ApplicationDbContext _db;

        public HomeViewModel HomeViewModel { get; set; }

        [BindProperty]
        public CreditApplication CreditModel { get; set; }

        public HomeController(IMarketForecaster marketForecaster,
            //IOptions<StripeSettings> stripeSettings,
            //IOptions<SendGridSettings> sendGridSettings,
            //IOptions<TwilioSettings> twilioSettings,
            IOptions<WazeForecastSettings> wazeForecastSettings,
            ICreditValidator creditValidator, 
            ApplicationDbContext db,
            ILogger<HomeController> logger,
            IUnitOfWork unitOfWork
            )
        {
            HomeViewModel = new HomeViewModel();
            _marketForecaster = marketForecaster;
            _creditValidator = creditValidator;
            _db = db;
            _logger = logger;
            _unitOfWork = unitOfWork;
            //_stripeSettings = stripeSettings.Value;
            //_sendGridSettings = sendGridSettings.Value;
            //_twilioSettings = twilioSettings.Value;
            _wazeForecastSettings = wazeForecastSettings.Value;
        }

        public IActionResult CreditApplication()
        {
            CreditModel = new CreditApplication();
            return View(CreditModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName(nameof(CreditApplication))]
        public async Task<IActionResult> CreditApplicationPost(
            [FromServices] Func<CreditApproveType, ICreditApproved> creditService)
        {
            if (ModelState.IsValid)
            {
                var (validationPassed, errorMessages) = await _creditValidator.PassAllValidations(CreditModel);
                CreditResultViewModel creditResultViewModel = new CreditResultViewModel()
                {
                    ErrorList = errorMessages,
                    CreditId = 0,
                    Success = validationPassed
                };

                if (validationPassed)
                {
                    CreditModel.CreditApproved =
                        creditService(CreditModel.Salary > 50000 ? CreditApproveType.High : CreditApproveType.Low)
                            .GetCreditApproved(CreditModel);
                    //add record to database
                    //await _db.CreditApplicationModel.AddAsync(CreditModel);
                    //await _db.SaveChangesAsync();
                    _unitOfWork.CreditApplicationRepository.Add(CreditModel);
                    _unitOfWork.Save();

                    creditResultViewModel.CreditId = CreditModel.Id;
                    creditResultViewModel.CreditApproved = CreditModel.CreditApproved;
                    return RedirectToAction(nameof(CreditResult), creditResultViewModel);
                }
                else
                {
                    return RedirectToAction(nameof(CreditResult), creditResultViewModel);
                }
            }
            return View(CreditModel);
        }

        public IActionResult CreditResult(CreditResultViewModel creditResultViewModel)
        {
            return View(creditResultViewModel);
        }


        public IActionResult Index()
        {
            _logger.LogInformation("Home Controller Index Action Called");
            //MarketForecasterV2 marketForecaster = new MarketForecasterV2();
            MarketResult currentMarketResult = _marketForecaster.GetMarketPrediction();
            switch (currentMarketResult.MarketCondition)
            {
                case MarketCondition.StableDown:
                    HomeViewModel.MarketForecast = "Market shows signs to go down in a stable state! It is a not a good sign to apply for credit applications! But extra credit is always piece of mind if you have handy when you need it.";
                    break;
                case MarketCondition.StableUp:
                    HomeViewModel.MarketForecast = "Market shows signs to go up in a stable state! It is a great sign to apply for credit applications!";
                    break;
                case MarketCondition.Volatile:
                    HomeViewModel.MarketForecast = "Market shows signs of volatility. In uncertain times, it is good to have credit handy if you need extra funds!";
                    break;
                default:
                    HomeViewModel.MarketForecast = "Apply for a credit card using our application!";
                    break;

            }
            _logger.LogInformation("Home Controller Index Action Ended");
            return View(HomeViewModel);
        }

        public IActionResult AllConfigSettings(
            [FromServices] IOptions<StripeSettings> stripeSettings,
            [FromServices] IOptions<SendGridSettings> sendGridSettings,
            [FromServices] IOptions<TwilioSettings> twilioSettings,
            [FromServices] IOptions<WazeForecastSettings> wazeForecastSettings
        )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastSettings.ForecastTrackerEnabled);
            messages.Add($"Stripe Publishable Key: " + stripeSettings.Value.PublishableKey);
            messages.Add($"Stripe Secret Key: " + stripeSettings.Value.SecretKey);
            messages.Add($"Send Grid Key: " + sendGridSettings.Value.SendGridKey);
            messages.Add($"Twilio Phone: " + twilioSettings.Value.PhoneNumber);
            messages.Add($"Twilio SID: " + twilioSettings.Value.AccountSid);
            messages.Add($"Twilio Token: " + twilioSettings.Value.AuthToken);
            return View(messages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
