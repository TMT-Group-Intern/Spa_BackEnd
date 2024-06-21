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
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<bool> DeleteUser(string Email)
        {
            await _userRepository.DeleteUser(Email);
            return true;
        }

        public async Task<string> GenerateAdminCodeAsync()
        {
            /*            var lastAdminCode = await _userRepository.GetLastAdminAsync();

                        if (lastAdminCode == null || lastAdminCode.AdminID == null)
                        {
                            return "AD0001";
                        }

                        string lastAdminCodeStr = lastAdminCode.AdminID.Value.ToString();
                        if (lastAdminCodeStr.Length < 3 || !lastAdminCodeStr.StartsWith("AD"))
                        {
                            throw new Exception("Invalid last admin code format");
                        }

                        long numericPart;
                        if (!long.TryParse(lastAdminCodeStr.Substring(2), out numericPart))
                        {
                            throw new Exception("Invalid last admin code format");
                        }

                        numericPart++;
                        return "AD" + numericPart.ToString("D4");*/
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

        public string GenerateToken(string Id, string Name, string Email, string Role)
        {
            var token = _userRepository.GenerateToken(Id, Name, Email, Role);
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

        public async Task<string> LoginAccount(string Email, string Password)
        {
            string token = await _userRepository.LoginAccount(Email, Password);
            return token;
        }

        public async Task<bool> UpdateAdmin(Admin AdminDTO)
        {
            await UpdateAdmin(AdminDTO);
            return true;
        }

        public async Task<bool> UpdateEmployee(Employee EmpDTO)
        {
            await UpdateEmployee(EmpDTO);
            return true;
        }

        public async Task<bool> UpdateUser(User UserDTO)
        {
            await UpdateUser(UserDTO);
            return true;
        }
        public bool isExistUser(string Email)
        {
            return _userRepository.GetUserByEmail(Email) == null ? false : true;
        }
    }
}
