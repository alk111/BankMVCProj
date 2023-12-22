using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Models
{
    public class Role
    {
        public virtual int Id { get; set; }
        public virtual string RoleName { get; set; }
        public virtual IList<User>  Users{ get; set; }
        

    }
}