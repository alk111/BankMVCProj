using BankMVC.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Mappings
{
    public class CustomerMap:ClassMap<Customer>
    {
        public CustomerMap() {
            Table("Customer");
            Id(o => o.Id);
            Map(o => o.FirstName);
            Map(o => o.LastName);
            Map(o => o.ContactNo);
            Map(o => o.Email);
            Map(o => o.IsActive);
            References(m => m.User).Columns("UserId").Cascade.All();
            //References(m => m.User).Columns("UserId").Nullable().Cascade.All();
            HasMany(m => m.Documents).Inverse().Cascade.SaveUpdate().KeyColumn("CustomerId");
            HasMany(m => m.Accounts).Inverse().Cascade.SaveUpdate().KeyColumn("CustomerId");
        }
    }
}