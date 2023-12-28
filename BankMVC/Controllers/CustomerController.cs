using BankMVC.Assemblers;
using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankMVC.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer

        private ICustomerService _customerService;
        private CustomerAssembler _customerAssembler;
        private IUserService _userService;
        private UserAssembler _userAssembler;
        public CustomerController(ICustomerService customerService, CustomerAssembler customerAssembler, IUserService userService , UserAssembler userAssembler)
        {
            _customerService = customerService;
            _customerAssembler = customerAssembler;
            _userService = userService;
            _userAssembler = userAssembler;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var customers = _customerService.GetAll();
            var customerVMs = customers.Select(c => _customerAssembler.ConvertToViewModel(c)).ToList();
            return View(customerVMs);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AdminDashboard()
        {
            return View();
        }
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerDashboard()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetData(int page, int rows, string sidx, string sord, string searchString)
        {
            var customers = _customerService.GetAll();

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
                        e.FirstName.Contains(searchString) ||
                        e.LastName.Contains(searchString) ||
                        e.ContactNo.ToString().Contains(searchString) ||
                        e.Email.Trim().Equals(searchString.Trim(), StringComparison.OrdinalIgnoreCase) ||
                        e.User.Id.ToString().Contains(searchString) ||
                        e.IsActive.ToString().Contains(searchString) ||
                        e.Accounts.Count.ToString().Contains(searchString) ||
                        e.Documents.Count.ToString().Contains(searchString)
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
                                        customer.FirstName,
                                        customer.LastName,
                                        customer.ContactNo.ToString(),
                                        customer.Email,
                                        customer.User.Id.ToString(),
                                        customer.IsActive.ToString(),
                                        customer.Accounts.Count.ToString(),
                                        customer.Documents.Count.ToString(),
                                        //customer.IsActive?"True":"False",
                                    }
                        }).Skip((page - 1) * rows).Take(rows).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //}
            //}

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(CustomerVM customerVM)
        {
            var userVM = new UserVM()
            {
                Username = customerVM.Username,
                Password = customerVM.Password,
                RoleId = 2
            };
            var newUser = _userAssembler.ConvertToModel(userVM);
            
            var customer = _customerAssembler.ConvertToModel(customerVM);
            customer.User = _userService.Add(newUser);
            
            var newCustomer = _customerService.Add(customer);
            ViewBag.Message = "Added Successfully";
            return View();
        }
        [HttpGet]
        public ActionResult Edit()
        {
            int id = (int)Session["LoginId"];
            var custData = _customerService.GetById(id);
            var custDataVM = _customerAssembler.ConvertToViewModel(custData);
            return View(custDataVM);
        }
        
        [HttpPost]
        public ActionResult Edit(CustomerVM customerVM)
        {
            var existingCustomer = _customerService.GetById(customerVM.Id);
            if (existingCustomer != null)
            {
                customerVM.UserId=existingCustomer.User.Id;
                customerVM.IsActive = true;
                var updatedCust = _customerAssembler.ConvertToModel(customerVM);
                _customerService.Update(updatedCust);

                return Json(new { success = true, message = "User updated successfully." });
            }


            return Json(new { success = false, message = "No such User Exists" });
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var existingCustomer = _customerService.GetById(id);
            if (existingCustomer != null)
            {

                _customerService.Delete(existingCustomer);
                //txn.Commit();
                return Json(new { success = true, message = "User deleted successfully." });
            }
            return Json(new { success = false, message = "No such User Exists" });
        }
    }
}