using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public interface ICreditApplicationRepository:IRepository<CreditApplication>
    {
        void Update(CreditApplication obj);
    }
}