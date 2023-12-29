using BankMVC.Assemblers;
using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankMVC.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        //private MyContext _myContext = new MyContext();
        // GET: Login
        private readonly IUserService _userService;
        private readonly UserAssembler _userAssembler;
        private readonly ICustomerService _customerService;
        private readonly ITransactionService _transactionService;
        public LoginController(IUserService userService, UserAssembler userAssembler, ICustomerService customerService, ITransactionService transactionService)
        {
            _userService = userService;
            _userAssembler = userAssembler;
            _customerService = customerService;
            _transactionService = transactionService;
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Login()
        {
            var strList = new List<string>();
            strList.Add("Empty");
            ViewBag.AccountNumbers = strList;
            return View();
        }
        [HttpPost]

        public ActionResult Login(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUsername(userVM.Username);
                //var result1 = _myContext.Users.Where(x => x.Name == user.Name && x.Password == user.Password).Include(x => x.Role).FirstOrDefault();
                User result = null;
                var getUser = _userService.GetUserWithRole(user.Id);
                if (userVM.Username == getUser.Username && VerifyPassword(userVM.Password,getUser.Password))
                {
                    result = getUser;
                }
                if (result != null)
                {
                    Session["User"] = result.Username;
                    //Session["Role"] = result.Role.RoleName;
                    Session["UserId"] = result.Id;
                    var customers = _customerService.GetAll();
                    var data = customers.Where(x => x.User.Id == user.Id).FirstOrDefault();
                    if(data == null)
                    {
                        ViewBag.Message = "Customer does not exist ";
                        return View(data);
                    }
                    Session["LoginId"] = data.Id;
                    FormsAuthentication.SetAuthCookie(result.Username, false);
                    if (result.Role.RoleName == "Admin")
                        return RedirectToAction("AdminDashboard", "Customer");
                    List<string> accounts = _transactionService.GetAccountNos(data.Id);
                    //HttpContext.Session.SetObjectAsJson("Accounts", accounts);
                    // Set the List<string> in session
                    Session["AccountNo"] = accounts;
                    //ViewBag.AccountNumbers = accounts;
                    
                    return RedirectToAction("CustomerDashboard", "Customer");
                }

                ViewBag.Message = "Username or Password does not match";

                return View();
            }
            else
            {
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // ComputeHash - returns byte array, convert it to a string
                byte[] enteredPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < enteredPasswordBytes.Length; i++)
                {
                    builder.Append(enteredPasswordBytes[i].ToString("x2"));
                }

                return builder.ToString() == hashedPassword;
            }
        }
    }
}