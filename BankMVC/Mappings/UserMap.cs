using BankMVC.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Mappings
{
    public class UserMap:ClassMap<User>
    {
        public UserMap() 
        {
            Table("Users");
            Id(x => x.Id);
            Map(x => x.Username);
            Map(x => x.Password);
            References(x => x.Role).Column("RoleId").Cascade.All();
            HasOne(x=>x.Customer).Cascade.All();
        }
    }
}