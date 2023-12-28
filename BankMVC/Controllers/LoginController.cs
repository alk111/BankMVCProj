using BankMVC.Assemblers;
using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public LoginController(IUserService userService, UserAssembler userAssembler, ICustomerService customerService)
        {
            _userService = userService;
            _userAssembler = userAssembler;
            _customerService = customerService;
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(UserVM userVM)
        {
            var user= _userService.GetUserByUsername(userVM.Username);
            //var result1 = _myContext.Users.Where(x => x.Name == user.Name && x.Password == user.Password).Include(x => x.Role).FirstOrDefault();
            User result = null;
            var getUser = _userService.GetUserWithRole(user.Id);
            if(userVM.Username== getUser.Username && userVM.Password==getUser.Password) 
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
                Session["LoginId"] = data.Id;
                FormsAuthentication.SetAuthCookie(result.Username, false);
                if (result.Role.RoleName == "Admin")
                    return RedirectToAction("AdminDashboard", "Customer");
                return RedirectToAction("CustomerDashboard", "Customer");
            }

            ViewBag.Message = "Username or Password does not match";

            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}