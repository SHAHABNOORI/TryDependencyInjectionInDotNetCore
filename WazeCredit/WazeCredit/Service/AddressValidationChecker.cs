using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class AddressValidationChecker : IValidationChecker
    {
        public bool ValidatorLogic(CreditApplication model)
        {
            return model.PostalCode > 0 && model.PostalCode < 99999;
        }

        public string ErrorMessage => "Location validation failed";
    }
}