using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IUserRepository
    {
        //
        //Get User By Email
        Task<User> GetUserByEmail(string email);
        //
        //Get All User
        Task<List<User>> GetAllUsers();
        //
        //Create User
        Task<User> CreateUser(User userDTO);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        //
        //Login
        Task<string> LoginAccount(string Email, string Password);
        //
        //Token
        string GenerateToken(string Id, string Name, string Email, string Role);
        //
        //Delete User
        Task<bool> DeleteUser(string Email);
        //
        //Update
        Task<bool> UpdateUser(User UserDTO);
        Task<bool> UpdateAdmin(Admin AdminDTO);
        Task<bool> UpdateEmployee(Employee EmpDTO);
        //
        //Get last
        Task<Admin> GetLastAdminAsync();
        Task<Employee> GetLastEmployeeAsync();
    }
}
