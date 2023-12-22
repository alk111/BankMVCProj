using BankMVC.Helpers;
using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public class AccountTypeRepository:IAccountTypeRepository
    {
        public string Add(AccountType accountType)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(accountType);
                    txn.Commit();

                }
            }
            return "Added Succesfully";
        }
        public string Update(AccountType accountType)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Update(accountType);
                    txn.Commit();

                }
            }
            return "Updated Succesfully";
        }
        public string Delete(AccountType accountType)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Delete(accountType);
                    txn.Commit();

                }
            }
            return "Deleted Succesfully";
        }
        public AccountType GetById(int accountTypeId)
        {
            AccountType accountType = null;
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    accountType = session.Load<AccountType>(accountTypeId);
                    txn.Commit();

                }
            }
            return accountType;
        }
        public List<AccountType> GetAll()
        {
            List<AccountType> accountType = new List<AccountType>() { };
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    accountType = session.Query<AccountType>().ToList();
                    txn.Commit();

                }
            }
            return accountType;
        }
    }
}