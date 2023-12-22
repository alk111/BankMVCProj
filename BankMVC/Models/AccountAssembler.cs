using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Models
{
    public class AccountAssembler
    {
        public Account ConvertToModel(AccountVM accountVM)
        {
            return new Account()
            {
                Id = accountVM.Id,
                AccountNo = accountVM.AccountNo,
                AccountType = new AccountType() { Id = accountVM.AccountTypeId },
                Balance = accountVM.Balance,
                Customer = new Customer() { Id = accountVM.CustomerId },
            };
        }
        public AccountVM ConvertToViewModel(Account account)
        {
            return new AccountVM()
            {
                Id = account.Id,
                AccountNo = account.AccountNo,
                AccountTypeId = account.AccountType.Id,
                Balance = account.Balance,
                CustomerId = account.Customer.Id,
            };
        }
    }
}