using BankMVC.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Mappings
{
    public class AccountTypeMap:ClassMap<AccountType>
    {
        public AccountTypeMap() 
        {
            Table("AccountType");
            Id(x => x.Id);
            Map(x => x.Type);
            HasMany(x => x.Accounts).Inverse().Cascade.SaveUpdate().KeyColumn("accountTypeId");
        }
    }
}