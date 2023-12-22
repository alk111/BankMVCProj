using BankMVC.Models;
using BankMVC.Services;
using BankMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Assemblers
{
    public class UserAssembler
    {
        public readonly IRoleService _roleService;
        public UserAssembler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public User ConvertToModel(UserVM userVM)
        {
            var role = _roleService.GetById(userVM.RoleId);
            return new User()
            {
                Id = userVM.Id,
                Username=userVM.Username,
                Password=userVM.Password,
                //Role=new Role() { Id=userVM.RoleId  , },
                Role = role,
                
            };
        }
        public UserVM ConvertToViewModel(User user)
        {
            return new UserVM()
            {
                Id=user.Id,
                Username = user.Username,
                Password=user.Password, 
                RoleId=user.Role.Id,
            };
        }
    }
}