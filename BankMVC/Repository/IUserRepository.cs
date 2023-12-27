using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public interface IUserRepository
    {
        User Add(User user);
        string Update(User user);
        string Delete(User user);    
        User GetById(int userId);
        List<User> GetAll();
        User GetUserWithRole(int id);
        User GetUserByUsername(string username);
    }
}