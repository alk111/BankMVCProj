using BankMVC.Models;
using BankMVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public string Add(Customer customer)
        {
            return _customerRepository.Add(customer);
        }
        public string Update(Customer customer)
        {
            return _customerRepository.Update(customer);
        }
        public string Delete(Customer customer)
        {
            return _customerRepository.Delete(customer);
        }
        public Customer GetById(int customerId)
        {
            return _customerRepository.GetById(customerId);
        }
        public List<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }
    }
}