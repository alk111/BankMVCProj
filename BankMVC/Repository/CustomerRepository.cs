using BankMVC.Helpers;
using BankMVC.Models;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        public string Add(Customer customer)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(customer);
                    txn.Commit();

                }
            }
            return "Added Succesfully";
        }
        public string Update(Customer customer)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Update(customer);
                    txn.Commit();

                }
            }
            return "Updated Succesfully";
        }
        public string Delete(Customer customer)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Delete(customer);
                    txn.Commit();

                }
            }
            return "Deleted Succesfully";
        }
        public Customer GetById(int custId)
        {
            Customer customer = null;
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    //customer = session.Load<Customer>(custId);
                    customer = session.Query<Customer>().Where(x=>x.Id == custId)
                        .Fetch(c => c.Documents)
                        .Fetch(c => c.Accounts)
                        .FirstOrDefault();
                    txn.Commit();

                }
            }
            return customer;
        }
        public List<Customer> GetAll()
        {
            List<Customer> customer = new List<Customer>() { };
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    customer = session.Query<Customer>()
                        .Fetch(c=>c.Documents)
                        .Fetch(c => c.Accounts)
                        .Fetch(x=>x.User)
                        .ToList();
                    txn.Commit();

                }
            }
            return customer;
        }
    }
}