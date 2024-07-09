using Spa.Domain.Authentication;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
     public interface IUserService
    {
        Task<string> GenerateAdminCodeAsync();
        Task<string> GenerateEmployeeCodeAsync();
        Task<List<User>> GetAllUsers();
        Task<List<Employee>> GetAllAdminsAndEmployees();
        Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize);
        Task<List<Employee>> GetAllEmployee();
        Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID);
        Task<List<Admin>> GetAllAdmin();
        Task<string> GetBranchName(long? branchID);
        Task<User> CreateUser(User userDTO); 
        Task<User> CreateUserForEmployee(string Email);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        Task<string> LoginAccount(string Email, string Password);
        Task<string> GenerateToken(string Id, string Name, string Email, string Role);
        Task DeleteUser(string Email);

        Task UpdateUser(User UserDTO);
        Task UpdateAdmin(Admin AdminDTO);
        Task UpdateEmployee(Employee EmpDTO);
        Task<User> GetUserByEmail(string email);
        Task<string> GetUserBoolByEmail(string email);
        Task<Admin> GetAdminByEmail(string email);
        Task<Employee> GetEmpByEmail(string email);
        Task<string> GetJobTypeName(long? JobTypeId);
        bool isExistUser(string email);
        Task<int> GetAllItem();
        Task<List<Branch>> GetAllBranches();
        Task<List<JobType>> GetAllJobs();
    }
}
