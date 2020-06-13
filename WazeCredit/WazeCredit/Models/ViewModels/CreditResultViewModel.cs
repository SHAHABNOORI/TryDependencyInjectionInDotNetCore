using System;
using System.Collections.Generic;

namespace WazeCredit.Models.ViewModels
{
    public class CreditResultViewModel
    {
        public bool Success { get; set; }

        public IEnumerable<String> ErrorList { get; set; }

        public int CreditId { get; set; }

        public double CreditApproved { get; set; }
    }
}