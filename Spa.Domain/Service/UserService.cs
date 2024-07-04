using Microsoft.AspNetCore.Identity;
using Spa.Domain.Authentication;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }
        public async Task<User> CreateUser(User userDTO)
        {
            if (userDTO.Role.Equals("Admin")) {
                //var adminCheck = await _userManager.FindByEmailAsync(userDTO.Email);
                //if (adminCheck != null) { throw new Exception("User Exist"); }
                var admin = await _userRepository.GetAdminByEmail(userDTO.Email);
                long? AdminID= admin.AdminID;
                userDTO.AdminID = AdminID;
                var newUser = await _userRepository.CreateUser(userDTO);
                return newUser;
            }
            else
            {
                var emp = await _userRepository.GetEmpByEmail(userDTO.Email);
                long? empID = emp.EmployeeID;
                userDTO.EmployeeID = empID;
                var newUser = await _userRepository.CreateUser(userDTO);
                return newUser;
            }
        }
        public async Task<User> CreateUserForEmployee(string Email)
        {
            var newUser = await _userRepository.CreateUserForEmployee(Email);
            return newUser;
        }

        public async Task CreateAdmin(Admin adminDTO)
        {
            var adminCheck = await _userRepository.GetAdminByEmail(adminDTO.Email);
            var empCheck = await _userRepository.GetEmpByEmail(adminDTO.Email);
            if (adminCheck != null|| empCheck is not null) { throw new Exception("null"); }
            var lastAdminID = await GenerateAdminCodeAsync();
            adminDTO.AdminCode = lastAdminID;
            await _userRepository.CreateAdmin(adminDTO);
        }

        public async Task CreateEmployee(Employee empDTO)
        {
            var empCheck = await _userRepository.GetEmpByEmail(empDTO.Email);
            var empCheckUser= await _userManager.FindByEmailAsync(empDTO.Email);
            if (empCheck != null||empCheckUser is not null) { throw new Exception(""); }
            var lastEmpID = await GenerateEmployeeCodeAsync();
            empDTO.EmployeeCode = lastEmpID;
            await _userRepository.CreateEmployee(empDTO);
        }

        public async Task DeleteUser(string Email)
        {
            await _userRepository.DeleteUser(Email);
        }

        public async Task<string> GenerateAdminCodeAsync()
        {
            var lastAdminCode = await _userRepository.GetLastAdminAsync();

            if (lastAdminCode == null)
            {
                return "AD0001";
            }
            var lastCode = lastAdminCode.AdminCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "AD" + numericPart.ToString("D4");
        }

        public async Task<string> GenerateEmployeeCodeAsync()
        {
            var lastEmployeeCode = await _userRepository.GetLastEmployeeAsync();

            if (lastEmployeeCode == null)
            {
                return "EM0001";
            }
            var lastCode = lastEmployeeCode.EmployeeCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "EM" + numericPart.ToString("D4");
        }

        public async Task<string> GenerateToken(string Id, string Name, string Email, string Role)
        {
            string token = await _userRepository.GenerateToken(Id, Name, Email, Role);
            return token;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users;
        }
        public async Task<List<Employee>> GetAllAdminsAndEmployees()
        {
           // var admin = await _userRepository.GetAllAdmin();
            var employ = await _userRepository.GetAllAdminsAndEmployees();

         
            return employ;
        }
        public async Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize)
        {
            var listUser = await _userRepository.GetByPages(pageNumber, pageSize);
            return listUser;
        }
        public async Task<List<Employee>> GetAllEmployee()
        {
            var emps = await _userRepository.GetAllEmployee();
            return emps;
        }
        public async Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID)
        {
            var emps = await _userRepository.GetEmployeeByBranchAndJob(branchID, jobTypeID);
            return emps;
        }
        public async Task<List<Admin>> GetAllAdmin()
        {
            var admins = await _userRepository.GetAllAdmin();
            return admins;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user;
        }
        public async Task<string> GetUserBoolByEmail(string email)
        {
            string checkUser = await _userRepository.GetUserBoolByEmail(email);
            return checkUser;
        }
        public async Task<Admin> GetAdminByEmail(string email)
        {
            var admin = await _userRepository.GetAdminByEmail(email);
            return admin;
        }
        public async Task<Employee> GetEmpByEmail(string email)
        {
            var emp = await _userRepository.GetEmpByEmail(email);
            return emp;
        }
        public async Task<string> GetJobTypeName(long? JobTypeId)
        {
            var Role = await _userRepository.GetJobTypeName(JobTypeId);
            return Role;
        }

        public async Task<string> LoginAccount(string Email, string Password)
        {
            string token = await _userRepository.LoginAccount(Email, Password);
            return token;
        }

        public async Task UpdateAdmin(Admin AdminDTO)
        {
            await _userRepository.UpdateAdmin(AdminDTO);
        }

        public async Task UpdateEmployee(Employee EmpDTO)
        {
            await _userRepository.UpdateEmployee(EmpDTO);
        }

        public async Task UpdateUser(User UserDTO)
        {
            await _userRepository.UpdateUser(UserDTO);
        }
        public bool isExistUser(string Email)
        {
            return _userRepository.GetUserByEmail(Email) == null ? false : true;
        }
        public async Task<int> GetAllItem()
        {
            return await _userRepository.GetAllItemProduct();
        }
        public async Task<List<JobType>> GetAllJobs()
        {
            var jobs = await _userRepository.GetAllJobs();
            return jobs;
        }
        public async Task<List<Branch>> GetAllBranches()
        {
            var brans = await _userRepository.GetAllBranches();
            return brans;
        }
        public async Task<string> GetBranchName(long? branchID)
        {
            var branch = await _userRepository.GetBranchName(branchID);
            return branch;
        }
    }
}
