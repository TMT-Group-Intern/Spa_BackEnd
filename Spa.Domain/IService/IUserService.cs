using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
     public interface IUserService
    {
        Task<string> GenerateAdminCodeAsync();  //generate Id Admin
        Task<string> GenerateEmployeeCodeAsync();  //generate Id Employee
        Task<List<User>> GetAllUsers(); //Get all
        Task<User> CreateUser(User userDTO); //Create User
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        Task<string> LoginAccount(string Email, string Password); //Login
        string GenerateToken(string Id, string Name, string Email, string Role); //Token
        Task<bool> DeleteUser(string Email);//Delete User
        Task<bool> UpdateUser(User UserDTO);//Update User
        Task<bool> UpdateAdmin(Admin AdminDTO);//Update Admin
        Task<bool> UpdateEmployee(Employee EmpDTO);//Update Employee
        Task<User> GetUserByEmail(string email);//Get User by Email
        bool isExistUser(string email);
    }
}
