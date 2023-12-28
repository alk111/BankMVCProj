using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Assemblers
{
    public class AccountAssembler
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountTypeService _accountTypeService;
        public AccountAssembler(ICustomerService customerService, IAccountTypeService accountTypeService)
        {
            _customerService = customerService;
            _accountTypeService = accountTypeService;
        }
        public Account ConvertToModel(AccountVM accountVM)
        {
            var cust = _customerService.GetById(accountVM.CustomerId);
            var accType = _accountTypeService.GetById(accountVM.AccountTypeId);
            return new Account()
            {
                Id = accountVM.Id,
                AccountNo = accountVM.AccountNo,
                //AccountType = new AccountType() { Id = accountVM.AccountTypeId },
                AccountType = accType,
                Balance = accountVM.Balance,
                IsActive = accountVM.IsActive,
                //Customer = new Customer() { Id = accountVM.CustomerId },
                Customer = cust
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
                IsActive = account.IsActive,
                TransactionsCount = account.Transactions != null ? account.Transactions.Count : 0,
                //AccountTypes = _accountTypeService.GetAll().Select(x => x.Type).ToList()
            };
        }
    }
}