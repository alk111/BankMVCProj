using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Assemblers
{
    public class DocumentAssembler
    {
        public readonly ICustomerService _customerService;
        public DocumentAssembler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public Document ConvertToModel(DocumentVM documentVM)
        {
            var cust = _customerService.GetById(documentVM.CustomerId);
            return new Document()
            {
                Id = documentVM.Id,
                DocumentName = documentVM.DocumentName,
                DocumentFile = documentVM.DocumentFile,
                //Customer = new Customer() { Id = documentVM.CustomerId },
                Customer = cust
            };
        }
        public DocumentVM ConvertToViewModel(Document document)
        {
            return new DocumentVM()
            {
                Id = document.Id,
                DocumentName = document.DocumentName,
                DocumentFile = document.DocumentFile,
                CustomerId = document.Customer.Id,
            };
        }
    }

}