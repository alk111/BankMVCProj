using BankMVC.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Mappings
{
    public class AccountMap:ClassMap<Account>
    {
        public AccountMap() 
        {
            Table("Account");
            Id(o => o.Id);
            Map(o => o.AccountNo);
            Map(o => o.Balance);
            Map(o => o.IsActive);
            References(m => m.AccountType).Column("AccountTypeId");
            References(m => m.Customer).Column("CustomerId");
        }
    }
}