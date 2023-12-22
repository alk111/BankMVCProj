using BankMVC.Helpers;
using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public class AccountRepository:IAccountRepository
    {
        public string Add(Account account)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(account);
                    txn.Commit();

                }
            }
            return "Added Succesfully";
        }
        public string Update(Account account)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Update(account);
                    txn.Commit();

                }
            }
            return "Updated Succesfully";
        }
        public string Delete(Account account)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Delete(account);
                    txn.Commit();

                }
            }
            return "Deleted Succesfully";
        }
        public Account GetById(int accountId)
        {
            Account account = null;
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    account = session.Load<Account>(accountId);
                    txn.Commit();

                }
            }
            return account;
        }
        public List<Account> GetAll()
        {
            List<Account> account = new List<Account>() { };
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    account = session.Query<Account>().ToList();
                    txn.Commit();

                }
            }
            return account;
        }
    }
}