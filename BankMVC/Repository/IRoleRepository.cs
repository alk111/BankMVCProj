using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public interface IRoleRepository
    {
        string Add(Role role);
        string Update(Role role);
        string Delete(Role role);
        Role GetById(int roleId);
        List<Role> GetAll();
    }
}