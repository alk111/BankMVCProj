using BankMVC.Assemblers;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        // GET: User
        private readonly IAccountService _accountService;
        private readonly AccountAssembler _accountAssembler;
        public AccountController(IAccountService accountService
        , AccountAssembler accountAssembler)
        {
            _accountService = accountService;
            _accountAssembler = accountAssembler;

        }
        public ActionResult Index()
        {
            var accounts = _accountService.GetAll();
            List<AccountVM> list = new List<AccountVM>();
            foreach (var account in accounts)
            {
                list.Add(_accountAssembler.ConvertToViewModel(account));
            }

            // Implement logic for displaying account types if needed
            return View(list);
            //return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(AccountVM accountVM)
        {
            ModelState.Remove("CustomerId");
            ModelState.Remove("AccountNo");
            if (ModelState.IsValid)
            {
                int length = 3;

                // creating a StringBuilder object()
                StringBuilder str_build = new StringBuilder();
                Random random = new Random();

                char letter;
                int shift = 0;
                int v = 0;
                for (int i = 0; i < length; i++)
                {
                    double flt = random.NextDouble();
                    v = random.Next(100, 999);
                    shift = Convert.ToInt32(Math.Floor(25 * flt));
                    letter = Convert.ToChar(shift + 65);

                    str_build.Append(letter);
                }
                str_build.Append(v.ToString());



                accountVM.AccountNo = str_build.ToString();
                accountVM.CustomerId =(int) Session["LoginId"];
                accountVM.IsActive = true;
                var account = _accountAssembler.ConvertToModel(accountVM);
                var newUser = _accountService.Add(account);
                ViewBag.Message = "Added Successfully";
                ViewBag.Status = "Successfull";
                return View();
            }
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
        public ActionResult Edit(AccountVM userVM)
        {
            var account = _accountService.GetById(userVM.Id);
            if (account != null)
            {
                var updatedData = _accountAssembler.ConvertToModel(userVM);
                _accountService.Update(updatedData);
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


        [HttpGet]
        public JsonResult GetData(int page, int rows, string sidx, string sord, string searchString)
        {
            if (User.IsInRole("Customer"))
            {
                int tempData = (int)Session["LoginId"];
                var customers = _accountService.GetAll();
                customers = customers.Where(x => x.Customer.Id == tempData).ToList();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    int searchId;
                    if (int.TryParse(searchString, out searchId))
                    {
                        // If the search term is a valid integer, search by Id
                        customers = customers.Where(e => e.Id == searchId).ToList();
                    }
                    else
                    {
                        // If the search term is not an integer, search by other fields
                        customers = customers.Where(e =>
                            e.AccountNo.Contains(searchString) ||
                            e.Balance.ToString().Contains(searchString) ||
                            e.AccountType.Type.ToString().ToLower().Contains(searchString.ToLower()) ||
                            e.Customer.FirstName.Contains(searchString.ToLower()) ||
                            e.IsActive.ToString().Contains(searchString) ||
                            e.Transactions.Count.ToString().Contains(searchString)

                        ).ToList();
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
                                        customer.AccountNo.ToString(),
                                        customer.Balance.ToString(),
                                        customer.AccountType.Type.ToString(),
                                        customer.Customer.FirstName.ToString(),
                                        customer.IsActive.ToString(),
                                        customer.Transactions.Count.ToString(),
                                       
                                        //customer.IsActive?"True":"False",
                                    }
                            }).Skip((page - 1) * rows).Take(rows).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);

            }
            else if (User.IsInRole("Admin"))
            {
                var customers = _accountService.GetAll();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    int searchId;
                    if (int.TryParse(searchString, out searchId))
                    {
                        // If the search term is a valid integer, search by Id
                        customers = customers.Where(e => e.Id == searchId).ToList();
                    }
                    else
                    {
                        // If the search term is not an integer, search by other fields
                        customers = customers.Where(e =>
                            e.AccountNo.Contains(searchString) ||
                            e.Balance.ToString().Contains(searchString) ||
                            e.AccountType.Type.ToString().ToLower().Contains(searchString.ToLower()) ||
                            e.Customer.FirstName.Contains(searchString.ToLower()) ||
                            e.IsActive.ToString().Contains(searchString) ||
                            e.Transactions.Count.ToString().Contains(searchString)

                        ).ToList();
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
                                        customer.AccountNo.ToString(),
                                        customer.Balance.ToString(),
                                        customer.AccountType.Type.ToString(),
                                        customer.Customer.FirstName.ToString(),
                                        customer.IsActive.ToString(),
                                        customer.Transactions.Count.ToString(),
                                       
                                        //customer.IsActive?"True":"False",
                                    }
                            }).Skip((page - 1) * rows).Take(rows).ToArray()

                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}