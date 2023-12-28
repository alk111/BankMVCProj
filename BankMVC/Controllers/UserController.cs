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
    public class UserController : Controller
    {
        
        // GET: User
        private readonly IUserService _userService;
        private readonly UserAssembler _userAssembler;
        public UserController(IUserService userService,UserAssembler userAssembler)
        {
            _userService = userService;
            _userAssembler = userAssembler;
        }

        public ActionResult Index()
        {
            var users = _userService.GetAll();
            List<UserVM> list = new List<UserVM>();
            foreach (var user in  users)
            {
                list.Add(_userAssembler.ConvertToViewModel(user));
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
        public ActionResult Create(UserVM userVM)
        {
            var user = _userAssembler.ConvertToModel(userVM);
            var newUser= _userService.Add(user);
            ViewBag.Message = "Added Successfully";
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userData = _userService.GetById(id);
            var userDataVM= _userAssembler.ConvertToViewModel(userData);
            return View(userDataVM);
        }
        [HttpPost]
        public ActionResult Edit(UserVM userVM)
        {
            var user = _userService.GetById(userVM.Id);
            if (user != null)
            {
                var updatedUser = _userAssembler.ConvertToModel(userVM);
               _userService.Update(updatedUser);
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult ChangePassword(UserVM userVM, string confirmNewPassword)
        {
            ModelState.Remove("Username");
            ModelState.Remove("RoleId");
            if (ModelState.IsValid)
            {
                if (userVM.Password != confirmNewPassword)
                {
                    //return Json(new { success = false, message = "New Password and Confirm Password not matching." });
                    ViewBag.Message = "New Password and Confirm Password not matching";
                    ViewBag.Status = "Unsuccessfull";
                    return View();
                }
                userVM.Id = (int)Session["UserId"];
                var user = _userService.GetById(userVM.Id);
                if (user != null)
                {
                    user.Password = confirmNewPassword;
                    //var updatedUser = _userAssembler.ConvertToModel(userVM);
                    _userService.Update(user);
                    ViewBag.Message = "Password Changed successfully.";
                    ViewBag.Status = "Successfull";
                    return View();
                }
                ViewBag.Message = "No such User Exists";
                ViewBag.Status = "Unsuccessfull";
                //return Json(new { success = true, message = "Password Changed Successfully." });
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var userData = _userService.GetById(id);
            var userDataVM = _userAssembler.ConvertToViewModel(userData);
            return View(userDataVM);
        }
        [HttpPost]
        public ActionResult Delete(UserVM userVM)
        {
            var user = _userService.GetById(userVM.Id);
            if (user != null)
            {
                _userService.Delete(user);
            }
            return RedirectToAction("Index");
        }
    }
}