using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankMVC.ViewModels
{
    public class UserVM
    {
        //[Required(ErrorMessage ="UserId is required")]
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(10,MinimumLength =4,ErrorMessage ="UserName should contain min 2 and max 10 alphabets")]
        public virtual string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(14, MinimumLength = 6, ErrorMessage = "UserName should contain min 6 and max 14 alphabets")]
        public virtual string Password { get; set; }
        [Required(ErrorMessage = "RoleId is required")]
        public virtual int RoleId { get; set; }
    }
}