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
        Task<string> GenerateAdminCodeAsync();  //generate Id Admin
        Task<string> GenerateEmployeeCodeAsync();  //generate Id Employee
        Task<List<User>> GetAllUsers(); //Get all
        Task<List<Employee>> GetAllAdminsAndEmployees();
        Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize); // quản lí phân trang
        Task<List<Employee>> GetAllEmployee();
        Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID);
        Task<List<Admin>> GetAllAdmin();
        Task<User> CreateUser(User userDTO); //Create User
        Task<User> CreateUserForEmployee(string Email);
        Task CreateAdmin(Admin adminDTO);
        Task CreateEmployee(Employee empDTO);
        Task<string> LoginAccount(string Email, string Password); //Login
        Task<string> GenerateToken(string Id, string Name, string Email, string Role); //Token
        Task DeleteUser(string Email);//Delete User

        Task UpdateUser(User UserDTO);//Update User
        Task UpdateAdmin(Admin AdminDTO);//Update Admin
        Task UpdateEmployee(Employee EmpDTO);//Update Employee
        Task<User> GetUserByEmail(string email);//Get User by Email
        Task<Admin> GetAdminByEmail(string email);
        Task<Employee> GetEmpByEmail(string email);
        Task<string> GetJobTypeName(long? JobTypeId);
        bool isExistUser(string email);
        Task<int> GetAllItem();
    }
}
