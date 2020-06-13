﻿using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class CreditApplicationRepository : Repository<CreditApplication>, ICreditApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public CreditApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CreditApplication obj)
        {
            _db.CreditApplicationModel.Update(obj);
        }
    }
}