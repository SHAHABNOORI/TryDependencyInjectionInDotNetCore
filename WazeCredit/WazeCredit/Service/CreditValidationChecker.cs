using Microsoft.VisualBasic;
using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditValidationChecker : IValidationChecker
    {
        public string ErrorMessage => "You did not meet Age/Salary/Credit requirements";

        public bool ValidatorLogic(CreditApplication model)
        {
            if (DateAndTime.Now.AddYears(-18) < model.Dob)
            {
                return false;
            }

            if (model.Salary < 10000)
            {
                return false;
            }

            return true;
        }

    }
}