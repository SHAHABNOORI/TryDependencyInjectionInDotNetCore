using System;

namespace WazeCredit.Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICreditApplicationRepository CreditApplicationRepository { get; }

        void Save();
    }
}