using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Assemblers
{
    public class TransactionAssembler
    {
        private readonly IAccountService _accountService;
        public TransactionAssembler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public TransactionVM ConvertToViewModel(Transaction transaction)
        {
            return new TransactionVM()
            {
                Id = transaction.Id,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                Date = transaction.Date,
                ToAccountNumber = transaction.ToAccountNumber,
                FromAccountNumber = transaction.FromAccountNumber,
                AccountId = transaction.Account.Id,
                
            };
        }

        public Transaction ConvertToModel(TransactionVM transactionVM)
        {
            var acc =_accountService.GetById(transactionVM.AccountId);
            return new Transaction()
            {
                Id = transactionVM.Id,
                TransactionType = transactionVM.TransactionType,
                Amount = transactionVM.Amount,
                Date = transactionVM.Date,
                ToAccountNumber = transactionVM.ToAccountNumber,
                FromAccountNumber = transactionVM.FromAccountNumber,
                //Account = new Account() { Id = transactionVM.AccountId },
                Account = acc,


            };
        }

    }
}