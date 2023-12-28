using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.ViewModels
{
    public class AccountVM
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Account Number is required.")]
        public virtual string AccountNo { get; set; }
        [Required(ErrorMessage = "Account Type is required.")]
        public virtual int AccountTypeId { get; set; }
        [Required(ErrorMessage = "Balance is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be non-negative.")]
        public virtual decimal Balance { get; set; }
        [Required(ErrorMessage = "CustomerId is required.")]
        public virtual int CustomerId { get; set; }
        public virtual int TransactionsCount { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual List<SelectListItem> AccountTypes { get; set; }
        public virtual string StoreAccountType { get; set; }
    }
}