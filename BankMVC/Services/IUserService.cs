﻿using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMVC.Services
{
    public interface IUserService
    {
        User Add(User user);
        string Update(User user);
        string Delete(User user);
        User GetById(int userId);
        List<User> GetAll();
        User GetUserWithRole(int id);
        User GetUserByUsername(string username);
        bool IsUniqueUserName(string username);
    }
}
