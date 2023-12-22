using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Models
{
    public class Account
    {
        public virtual int Id { get; set; }
        public virtual string AccountNo { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual Decimal Balance { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ISet<Transaction> Transactions { get; set; }
        public virtual bool IsActive { get; set; } 
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}