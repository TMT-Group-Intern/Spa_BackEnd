using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Spa.Domain.Authentication;
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
        Task<Admin> GetAdminByEmail(string email);
        Task<Employee> GetEmpByEmail(string email);
        //
        //Get All User
        Task<List<User>> GetAllUsers();
        Task<List<Employee>> GetAllAdminsAndEmployees();
        Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize); // quản lí phân trang
        Task<List<Employee>> GetAllEmployee();
        Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID);
        Task<List<Admin>> GetAllAdmin();
        //
        //Create User
        Task<User> CreateUser(User userDTO);
        Task<User> CreateUserForEmployee(string Email);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        //
        //Login
        Task<string> LoginAccount(string Email, string Password);
        //
        //Token
        Task<string> GenerateToken(string Id, string Name, string Email, string Role);
        //
        //Delete User
        Task<bool> DeleteUser(string Email);
        //
        //Update
        Task<bool> UpdateUser(User UserDTO);
        Task<bool> UpdateAdmin(Admin AdminDTO);
        Task<bool> UpdateEmployee(Employee EmpDTO);
        Task<string> GetJobTypeName(long? JobTypeId);
        //
        //Get last
        Task<Admin> GetLastAdminAsync();
        Task<Employee> GetLastEmployeeAsync();
        Task<int> GetAllItemProduct();
    }
}
