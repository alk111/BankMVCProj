using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Services
{
    public interface IRoleService
    {
        string Add(Role role);
        string Update(Role role);
        string Delete(Role role);
        Role GetById(int roleId);
        List<Role> GetAll();
    }
}