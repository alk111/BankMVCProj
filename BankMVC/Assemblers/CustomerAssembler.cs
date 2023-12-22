using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Assemblers
{
    public class CustomerAssembler
    {
        private readonly IUserService _userService;
        public CustomerAssembler(IUserService userService)
        {
            _userService = userService;
        }
        public Customer ConvertToModel(CustomerVM vm)
        {
            var user = _userService.GetById(vm.UserId);
            return new Customer
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                ContactNo = vm.ContactNo,
                Email = vm.Email,
                IsActive = vm.IsActive,
                User = user,
            };
        }

        public CustomerVM ConvertToViewModel(Customer customer)
        {
            return new CustomerVM
            {
                Id= customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                ContactNo = customer.ContactNo,
                Email = customer.Email,
                IsActive = customer.IsActive,
                DocumentsCount = customer.Documents.Count,
                AccountsCount = customer.Accounts.Count,
            };
        }
    }
}