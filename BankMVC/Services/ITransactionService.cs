using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Services
{
    public interface ITransactionService
    {
        string Add(Transaction transaction);
        string Update(Transaction transaction);
        string Delete(Transaction transaction);
        Transaction GetById(int transactionId);
        List<Transaction> GetAll();
        List<Transaction> GetAllByCustFilter(int tempData);
        List<string> GetAccountNos(int id);
    }
}