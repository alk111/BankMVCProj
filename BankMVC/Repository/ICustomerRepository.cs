using BankMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Repository
{
    public interface ICustomerRepository
    {
        string Add(Customer customer);
        string Update(Customer customer);
        string Delete(Customer customer);
        Customer GetById(int custId);
        List<Customer> GetAll();
    }
}