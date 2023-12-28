using BankMVC.Models;
using BankMVC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BankMVC.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User Add(User user)
        {
            user.Password=HashPassword(user.Password);
           return _userRepository.Add(user);
        }
        public string Update(User user)
        {
            user.Password = HashPassword(user.Password);
            return _userRepository.Update(user);
        }
        public string Delete(User user)
        {
            return _userRepository.Delete(user);
        }
        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }
        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }
        public User GetUserWithRole(int id)
        {
            return _userRepository.GetUserWithRole(id);
        }
        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // ComputeHash - returns byte array, convert it to a string
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public bool IsUniqueUserName(string username)
        {
            var usernameExist = _userRepository.GetUserByUsername(username);
            if (usernameExist != null && usernameExist.Username == username)
            {
                return false;
            }
            return true;
        }
    }
}