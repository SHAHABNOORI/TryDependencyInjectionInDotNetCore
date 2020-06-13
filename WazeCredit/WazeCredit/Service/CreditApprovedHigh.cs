using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedHigh : ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication)
        {
            //Complex logic
            return creditApplication.Salary * 0.3;
        }
    }
}