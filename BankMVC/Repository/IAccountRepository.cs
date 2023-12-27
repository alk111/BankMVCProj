using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public interface IAccountRepository
    {
        string Add(Account account);
        string Update(Account account);
        string Delete(Account account);
        Account GetById(int accountId);
        List<Account> GetAll();
        Account GetByAccountNumber(string accNo);
    }
}