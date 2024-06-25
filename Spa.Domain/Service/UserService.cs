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


        /*        public async Task<User> CreateUser(User userDTO)
                {
                    if (userDTO.Role == "Admin")
                    {
                        var lastAdminID = await GenerateAdminCodeAsync();
                        userDTO.Code = lastAdminID;
                        var newAdmin = await _userRepository.CreateUser(userDTO);
                        return newAdmin;
                    }
                    else
                    {
                        var lastEmpID = await GenerateEmployeeCodeAsync();
                        userDTO.Code = lastEmpID;
                        var newEmployee = await _userRepository.CreateUser(userDTO);
                        return newEmployee;
                    }
                }*/


        public async Task<User> CreateUser(User userDTO)
        {
            if (userDTO.Role.Equals("Admin")) {
                var lastAdminID = await GenerateAdminCodeAsync();
                userDTO.Code = lastAdminID;
                var newUser = await _userRepository.CreateUser(userDTO);
                return newUser;
            }
            else
            {
                var lastEmpID = await GenerateEmployeeCodeAsync();
                userDTO.Code = lastEmpID;
                var newUser = await _userRepository.CreateUser(userDTO);
                return newUser;
            }
        }

        public async Task CreateAdmin(Admin adminDTO)
        {
/*            var lastAdminID = await GenerateAdminCodeAsync();
            adminDTO.AdminCode = lastAdminID;*/
            await _userRepository.CreateAdmin(adminDTO);
            //return newAdmin;
        }

        public async Task CreateEmployee(Employee empDTO)
        {
/*            var lastEmpID = await GenerateEmployeeCodeAsync();
            empDTO.EmployeeCode = lastEmpID;*/
            await _userRepository.CreateEmployee(empDTO);
            //return newEmp;
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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user;
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
    }
}
