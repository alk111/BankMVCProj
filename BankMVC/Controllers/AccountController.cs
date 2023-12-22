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
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly AccountAssembler _accountAssembler;
        public AccountController(IAccountService accountService, AccountAssembler accountAssembler)
        {
            _accountService = accountService;
            _accountAssembler = accountAssembler;
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
        public ActionResult Create(AccountVM accountVM)
        {
            var account = _accountAssembler.ConvertToModel(accountVM);
            var newUser = _accountService.Add(account);
            ViewBag.Message = "Added Successfully";
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var accountData = _accountService.GetById(id);
            var accountDataVM = _accountAssembler.ConvertToViewModel(accountData);
            return View(accountDataVM);
        }
        [HttpPost]
        public ActionResult Edit(AccountVM accountVM)
        {
            var account = _accountService.GetById(accountVM.Id);
            if (account != null)
            {
                _accountService.Update(account);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var accountData = _accountService.GetById(id);
            var accountDataVM = _accountAssembler.ConvertToViewModel(accountData);
            return View(accountDataVM);
        }
        [HttpPost]
        public ActionResult Delete(AccountVM accountVM)
        {
            var account = _accountService.GetById(accountVM.Id);
            if (account != null)
            {
                _accountService.Delete(account);
            }
            return RedirectToAction("Index");
        }
    }
}