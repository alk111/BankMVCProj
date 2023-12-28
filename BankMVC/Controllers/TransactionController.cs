using BankMVC.Assemblers;
using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace BankMVC.Controllers
{
    [AllowAnonymous]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly TransactionAssembler _transactionAssembler;
        public TransactionController(ITransactionService transactionService, TransactionAssembler transactionAssembler, IAccountService accountService)
        {
            _transactionService = transactionService;
            _transactionAssembler = transactionAssembler;
            _accountService = accountService;

        }
        public ActionResult Index()
        {
            var transactions = _transactionService.GetAll();
            var transactionVMs = transactions.Select(c => _transactionAssembler.ConvertToViewModel(c)).ToList();
            return View(transactionVMs);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TransactionVM transactionVM)
        {
            
            if (transactionVM.TransactionType == "Debit")
            {
                transactionVM.ToAccountNumber = null;
                Withdraw(transactionVM);
                return RedirectToAction("CustomerDashboard","Customer");
            }
            else if(transactionVM.TransactionType == "Credit")
            {
                transactionVM.FromAccountNumber = null;
                Deposit(transactionVM);
                return RedirectToAction("CustomerDashboard", "Customer");
            }
            else if (transactionVM.TransactionType =="Transfer")
            {
                Withdraw(transactionVM);
                Deposit(transactionVM);
                return RedirectToAction("CustomerDashboard", "Customer");
            }
            return Json(new { success = false, message = "Error Occured" });
        }
        
        [HttpPost]
        public ActionResult Edit(TransactionVM transactionVM)
        {
            var transaction = _transactionService.GetById(transactionVM.Id);
            if (transaction != null)
            {
                transaction.Amount = transactionVM.Amount;
                transaction.TransactionType = transactionVM.TransactionType;
                transaction.Date = transactionVM.Date;
                transaction.FromAccountNumber = transactionVM.FromAccountNumber;
                transaction.ToAccountNumber = transactionVM.ToAccountNumber;

                //var updatedData=_transactionAssembler.ConvertToModel(transactionVM);
                _transactionService.Update(transaction);
                return Json(new { success = true, message = "User updated successfully." });
            }
            return Json(new { success = false, message = "No such User Exists" });
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

        [HttpGet]
        public JsonResult GetData(int page, int rows, string sidx, string sord, string searchString)
        {
            List<Transaction> customers = null;
            int tempData = (int)Session["LoginId"];
            if (User.IsInRole("Customer"))
            {
                customers = _transactionService.GetAllByCustFilter(tempData);
            }
            else
            {
                customers = _transactionService.GetAll();
            }
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                int searchId;
                if (int.TryParse(searchString, out searchId))
                {
                    // If the search term is a valid integer, search by Id
                    customers = customers.Where(e => e.Id == searchId || e.TransactionType.Contains(searchString)).ToList();
                }
                else
                {
                    // If the search term is not an integer, search by FirstName or LastName
                    customers = customers.Where(e => e.TransactionType.Contains(searchString)).ToList();
                }
            }
            // Get total count of records (for pagination)
            int totalCount = customers.Count();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalCount / rows);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalCount,
                rows = (from customer in customers
                        orderby sidx + " " + sord
                        select new
                        {
                            cell = new string[] {
                                        customer.Id.ToString(),
                                        customer.TransactionType,
                                        customer.Amount.ToString(),
                                        customer.Date.ToString(),
                                        customer.FromAccountNumber,
                                        customer.ToAccountNumber,
                                    }
                        }).Skip((page - 1) * rows).Take(rows).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadExcel()

        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Transactions");

            // Add headers
            var headers = new List<string> { "AccountId", "TransactionType", "Amount", "Date", "ToAccountNumber", "FromAccountNumber", };
            for (var i = 1; i <= headers.Count; i++)
            {
                worksheet.Cells[1, i].Value = headers[i - 1];
            }

            // Add data
            var rowIndex = 2;
            var transactions = _transactionService.GetAll();
            foreach (var transaction in transactions)
            {
                worksheet.Cells[rowIndex, 1].Value = transaction.Account.Id;
                worksheet.Cells[rowIndex, 2].Value = transaction.TransactionType;
                worksheet.Cells[rowIndex, 3].Value = transaction.Amount;
                worksheet.Cells[rowIndex, 4].Value = transaction.Date.ToString();
                worksheet.Cells[rowIndex, 5].Value = transaction.ToAccountNumber;
                worksheet.Cells[rowIndex, 6].Value = transaction.FromAccountNumber;


                rowIndex++;
            }

            // Return the Excel file
            var memoryStream = new MemoryStream(package.GetAsByteArray());
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transactions.xlsx");
        }

        [HttpGet]
        public ActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(TransactionVM transactionVM)
        {
            ModelState.Remove("FromAccountNumber");
            ModelState.Remove("AccountId");
            ModelState.Remove("TransactionType");
            ModelState.Remove("Date");
            if (ModelState.IsValid)
            {
                //var account = _accountService.GetById(transactionVM.AccountId);
                var account = _accountService.GetByAccountNumber(transactionVM.ToAccountNumber);
                if (account != null)
                {
                    transactionVM.TransactionType = "Credit";
                    transactionVM.Date = DateTime.Now;
                    transactionVM.AccountId = account.Id;
                    account.Balance = account.Balance + transactionVM.Amount;
                    _accountService.Update(account);
                    var transaction = _transactionAssembler.ConvertToModel(transactionVM);
                    var newTransaction = _transactionService.Add(transaction);
                    //return Json(new { success = true, message = "Amount Deposited Successfully." });
                    ViewBag.Message = "Amount Deposited Successfully.";
                    ViewBag.Status = "Successfull";
                    return View();
                }
                ViewBag.Message = "No such Account Found.";
                ViewBag.Status = "Unsuccessfull";
                //return Json(new { success = false, message = "No such Account Found." });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Withdraw()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Withdraw(TransactionVM transactionVM)
        {
            ModelState.Remove("ToAccountNumber");
            ModelState.Remove("AccountId");
            ModelState.Remove("TransactionType");
            ModelState.Remove("Date");
            if (ModelState.IsValid)
            {
                //transactionVM.ToAccountNumber = null;
                //var account = _accountService.GetById(transactionVM.AccountId);
                var account = _accountService.GetByAccountNumber(transactionVM.FromAccountNumber);
                if (account != null)
                {
                    transactionVM.TransactionType = "Debit";
                    transactionVM.Date = DateTime.Now;
                    transactionVM.AccountId = account.Id;
                    if (account.Balance > transactionVM.Amount)
                    {
                        account.Balance = account.Balance - transactionVM.Amount;
                        _accountService.Update(account);
                        var transaction = _transactionAssembler.ConvertToModel(transactionVM);
                        var newTransaction = _transactionService.Add(transaction);
                        //return Json(new { success = true, message = "Amount Withdrawn Successfully." });
                        ViewBag.Message = "Amount Withdrawn Successfully.";
                        ViewBag.Status = "Successfull";
                        return View();
                    }
                    ViewBag.Message = "Insufficent Balance.";
                    ViewBag.Status = "Unsuccessfull";
                    return View();
                    //return Json(new { success = false, message = "Insufficent Balance." });
                }
                ViewBag.Message = "No such Account Found.";
                ViewBag.Status = "Unsuccessfull";
                //return Json(new { success = false, message = "No such Account Found." });
            }
            return View();

        }
       

        [HttpGet]
        public ActionResult Transfer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Transfer(TransactionVM transactionVM)
        {
            ModelState.Remove("AccountId");
            ModelState.Remove("TransactionType");
            ModelState.Remove("Date");
            if (ModelState.IsValid)
            {

                Withdraw(transactionVM);
                if(ViewBag.Status!= "Successfull")
                {
                    ViewBag.Message = "Withdraw Failed";
                    ViewBag.Status = "Unsuccessfull";
                    return View();
                }
                Deposit(transactionVM);
                if (ViewBag.Status != "Successfull")
                {
                    ViewBag.Message = "Deposit Failed";
                    ViewBag.Status = "Unsuccessfull";
                    return View();
                }

                //return Json(new { success = true, message = "Amount Transferd Successfully." });
                ViewBag.Message = "Amount Transferd Successfully.";
                ViewBag.Status = "Successfull";
                return View();
            }
            return View();
        }

    }
}