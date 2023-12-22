using BankMVC.Helpers;
using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public class TransactionRepository:ITransactionRepository
    {
        public string Add(Transaction transaction)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Save(transaction);
                    txn.Commit();

                }
            }
            return "Added Succesfully";
        }
        public string Update(Transaction transaction)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Update(transaction);
                    txn.Commit();

                }
            }
            return "Updated Succesfully";
        }
        public string Delete(Transaction transaction)
        {
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    session.Delete(transaction);
                    txn.Commit();

                }
            }
            return "Deleted Succesfully";
        }
        public Transaction GetById(int transactionId)
        {
            Transaction transaction = null;
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    transaction = session.Load<Transaction>(transactionId);
                    txn.Commit();

                }
            }
            return transaction;
        }
        public List<Transaction> GetAll()
        {
            List<Transaction> transaction = new List<Transaction>() { };
            using (var session = NHibernateHelpers.OpenSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    transaction = session.Query<Transaction>().ToList();
                    txn.Commit();

                }
            }
            return transaction;
        }
    }
}