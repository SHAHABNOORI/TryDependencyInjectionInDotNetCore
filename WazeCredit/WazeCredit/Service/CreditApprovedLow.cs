using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedLow:ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication)
        {
            //less logic
            return creditApplication.Salary * 0.5;
        }
    }
}