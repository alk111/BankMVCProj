using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Services
{
    public interface IAccountTypeService
    {
        string Add(AccountType accountType);
        string Update(AccountType accountType);
        string Delete(AccountType accountType);
        AccountType GetById(int accountTypeId);
        List<AccountType> GetAll();
        AccountType GetByType(string Type);
    }
}