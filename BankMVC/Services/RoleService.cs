using BankMVC.Models;
using BankMVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankMVC.Services
{
    public class RoleService:IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public string Add(Role role)
        {
            return _roleRepository.Add(role);
        }
        public string Update(Role role)
        {
            return _roleRepository.Update(role);
        }
        public string Delete(Role role)
        {
            return _roleRepository.Delete(role);
        }
        public Role GetById(int roleId)
        {
            return _roleRepository.GetById(roleId);
        }
        public List<Role> GetAll()
        {
            return _roleRepository.GetAll();
        }
    }
}
