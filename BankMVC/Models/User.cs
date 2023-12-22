using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BankMVC.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set;}
        public virtual string Password { get; set;}
        public virtual Role Role { get; set;}

        //1-1 relation
        public virtual Customer Customer { get; set;}
        //public virtual ISet<Customer> Customers { get; set; }
        //public User()
        //{
        //    Customers = new HashSet<Customer>();
        //}
        

    }
}