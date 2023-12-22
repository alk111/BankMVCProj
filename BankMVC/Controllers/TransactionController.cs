using BankMVC.Assemblers;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly TransactionAssembler _transactionAssembler;
        public TransactionController(ITransactionService transactionService, TransactionAssembler transactionAssembler)
        {
            _transactionService = transactionService;
            _transactionAssembler = transactionAssembler;
        }
        public ActionResult Index()
        {
          
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TransactionVM transactionVM)
        {
            var transaction = _transactionAssembler.ConvertToModel(transactionVM);
            var newTransaction = _transactionService.Add(transaction);
            ViewBag.Message = "Added Successfully";
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var transactionData = _transactionService.GetById(id);
            var transactionDataVM = _transactionAssembler.ConvertToViewModel(transactionData);
            return View(transactionDataVM);
        }
        [HttpPost]
        public ActionResult Edit(TransactionVM transactionVM)
        {
            var transaction = _transactionService.GetById(transactionVM.Id);
            if (transaction != null)
            {
                _transactionService.Update(transaction);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var transactionData = _transactionService.GetById(id);
            var transactionDataVM = _transactionAssembler.ConvertToViewModel(transactionData);
            return View(transactionDataVM);
        }
        [HttpPost]
        public ActionResult Delete(TransactionVM transactionVM)
        {
            var transaction = _transactionService.GetById(transactionVM.Id);
            if (transaction != null)
            {
                _transactionService.Delete(transaction);
            }
            return RedirectToAction("Index");
        }
    }
}