using BankMVC.Models;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Assemblers
{
    public class AccountTypeAssembler
    {
        public AccountType ConvertToModel(AccountTypeVM accountTypeVM)

        {
            return new AccountType()
            {
                Id = accountTypeVM.Id,
                Type = accountTypeVM.Type,
                                
            };








        }
        public AccountTypeVM ConvertToViewModel(AccountType accountType)
        {
            return new AccountTypeVM()
            {
                Id = accountType.Id,
                Type = accountType.Type,
                AccountsCount=accountType.Accounts!=null ? accountType.Accounts.Count() : 0,
            };
        }
    }
}