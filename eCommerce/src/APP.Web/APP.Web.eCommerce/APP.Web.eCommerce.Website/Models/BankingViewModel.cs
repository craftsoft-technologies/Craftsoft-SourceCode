using System;
using System.Collections.Generic;

namespace APP.Web.eCommerce.Website.Models
{
    public enum TransactionTypeEnum
    {
        Deposit = 1,
        Withdrawal = 2
    }
    public class BankingViewModel
    {
        public string BankName { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public decimal Amount { get; set; }
    }

}