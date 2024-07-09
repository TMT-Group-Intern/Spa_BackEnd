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
        Task<User> GetUserByEmail(string email);
        Task<string> GetUserBoolByEmail(string email);
        Task<Admin> GetAdminByEmail(string email);
        Task<Employee> GetEmpByEmail(string email);
        Task<List<User>> GetAllUsers();
        Task<List<Employee>> GetAllAdminsAndEmployees();
        Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize);
        Task<List<Employee>> GetAllEmployee();
        Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID);
        Task<List<Admin>> GetAllAdmin();
        Task<User> CreateUser(User userDTO);
        Task<string> GetBranchName(long? branchID);
        Task<User> CreateUserForEmployee(string Email);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        Task<string> LoginAccount(string Email, string Password);
        Task<string> GenerateToken(string Id, string Name, string Email, string Role);
        Task<bool> DeleteUser(string Email);
        Task<bool> UpdateUser(User UserDTO);
        Task<bool> UpdateAdmin(Admin AdminDTO);
        Task<bool> UpdateEmployee(Employee EmpDTO);
        Task<string> GetJobTypeName(long? JobTypeId);
        Task<Admin> GetLastAdminAsync();
        Task<Employee> GetLastEmployeeAsync();
        Task<int> GetAllItemProduct();
        Task<List<Branch>> GetAllBranches();
        Task<List<JobType>> GetAllJobs();
    }
}
