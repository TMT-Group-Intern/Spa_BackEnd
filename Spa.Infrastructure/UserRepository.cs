using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Spa.Domain.IRepository;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
using NMemory.Linq;
using Azure.Core;

namespace Spa.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private static readonly List<User> _user = new();
        private static readonly List<Admin> _admin = new();
        private static readonly List<Employee> _employee = new();
        private readonly IConfiguration _config;
        private readonly SpaDbContext _spaDbContext;
        public UserRepository(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SpaDbContext spaDbContext, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _spaDbContext = spaDbContext;
        }



        //
        //Get User By Email
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        //
        //Get All User
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users is null)
            {
                return null;
            }

            var userDTOs = _user;

            foreach (var user in users)
            {
                var userDTO = new User
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    PhoneNumber = user.PhoneNumber,
                    Code = user.Code,
                };

                userDTOs.Add(userDTO);
            }
            userDTOs = userDTOs.OrderBy(u => u.Role).ToList();
            return userDTOs;
        }

        public async Task<User> CreateUser(User userDTO)
        {
            if (userDTO is null) return null;
            var newUser = new User()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
                Role = userDTO.Role,
                Code = userDTO.Code,
                UserName = userDTO.Email,
                PhoneNumber= userDTO.PhoneNumber,
            };
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
            {
                throw new Exception("User exist!");
            }

            var createUser = await _userManager.CreateAsync(newUser!, userDTO.PasswordHash);
            if (!createUser.Succeeded)
            {
                throw new Exception("Failed to create user");
            }
            return newUser;
        }
        public async Task CreateAdmin(Admin adminDTO) 
        {
                var newAdmin = new Admin()
                {
                    Email = adminDTO.Email,
                    FirstName = adminDTO.FirstName,
                    LastName = adminDTO.LastName,
                    Password = adminDTO.Password,
                    Role = adminDTO.Role,
                    AdminCode = adminDTO.AdminCode,
                    Id = adminDTO.Id,
                    Phone = adminDTO.Phone,
                    DateOfBirth = adminDTO.DateOfBirth,
                    Gender = adminDTO.Gender,
                };
                await _spaDbContext.Admins.AddAsync(newAdmin);
                await _spaDbContext.SaveChangesAsync();
        }            
        
        public async Task CreateEmployee(Employee empDTO)
        {
/*            var userEmployee = await _spaDbContext.Employees.FindAsync(empDTO.Email);
            if (userEmployee is not null)
            {
                throw new Exception("User exist!");
            }*/
            var newEmployee = new Employee()
            {
                Email = empDTO.Email,
                FirstName = empDTO.FirstName,
                LastName = empDTO.LastName,
                Password = empDTO.Password,
                EmployeeCode = empDTO.EmployeeCode,
                Id = empDTO.Id,
                Phone = empDTO.Phone,
                DateOfBirth = empDTO.DateOfBirth,
                Gender = empDTO.Gender,
                HireDate = empDTO.HireDate,
                JobTypeID = empDTO.JobTypeID,
                BranchID = empDTO.BranchID
            };
            await _spaDbContext.Employees.AddAsync(newEmployee);
            await _spaDbContext.SaveChangesAsync();
            //return newEmployee;
        }

        //
        //Login
        public async Task<string> LoginAccount(string Email, string Password)
        {
            if (Email is null || Password is null)
                return "Empty";

            var getUser = await _userManager.FindByEmailAsync(Email);
            if (getUser is null)
                return "Email not exist!";
            bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, Password);
            if (!checkUserPasswords)
                return "Invalid password!";
            string token = GenerateToken(getUser.Code, (getUser.FirstName + getUser.LastName), getUser.Email, getUser.Role);
            //string token = getUser.Email;
            return token;
        }

        //
        //Token
        /*
            "Jwt": {
                    "Key": "qaz1wsx2edc3rfv4tgb5yhn6ujm7ik,8ol.9p;/0qwertyuiop",
                    "Issuer": "https://localhost:7192",
                    "Audience": "https://localhost:7192"
            }
        */
        public string GenerateToken(string Id, string Name, string Email, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //claim information
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Id),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.Role, Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //
        //Delete User
        public async Task<bool> DeleteUser(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return false;
            }

            await _userManager.DeleteAsync(user);

            if (user.Role == "Admin")
            {
                var admin = new Admin();
                admin = await _spaDbContext.Admins.FindAsync(Email);
                _spaDbContext.Admins.Remove(admin);
                _spaDbContext.SaveChanges();
            }
            else
            {
                var emp = new Employee();
                emp = await _spaDbContext.Employees.FindAsync(Email);
                _spaDbContext.Employees.Remove(emp);
                _spaDbContext.SaveChanges();
            }
            return true;
        }

        //
        //Update
        public async Task<bool> UpdateUser(User UserDTO)
        {
            if (UserDTO is null) return false;

            var newUpdate = new User
            {
                FirstName = UserDTO.FirstName,
                LastName = UserDTO.LastName,
                Email = UserDTO.Email,
                PasswordHash = UserDTO.PasswordHash,
                Role = UserDTO.Role
            };
            var userUpdate = await _userManager.FindByEmailAsync(UserDTO.Email);
            if (userUpdate is null) return false;


            userUpdate.FirstName = UserDTO.FirstName;
            userUpdate.LastName = UserDTO.LastName;
            userUpdate.Email = UserDTO.Email;
            userUpdate.Role = UserDTO.Role;

            //user.PasswordHash = updateUserDTO.Password;
            var passwordHasher = new PasswordHasher<User>();
            userUpdate.PasswordHash = passwordHasher.HashPassword(userUpdate, UserDTO.PasswordHash);
            var updateUserResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateUserResult.Succeeded)
                return false;
            return true;
        }

        public async Task<bool> UpdateAdmin(Admin AdminDTO)
        {
            if (AdminDTO is null) return false;

            var newUpdate = new Admin
            {
                //AdminID = AdminDTO.AdminID,
                FirstName = AdminDTO.FirstName,
                LastName = AdminDTO.LastName,
                Email = AdminDTO.Email,
                Password = AdminDTO.Password,
                Phone = AdminDTO.Phone,
                DateOfBirth = AdminDTO.DateOfBirth,
                Gender = AdminDTO.Gender,
                //AdminCode = AdminDTO.AdminCode,
                Role = AdminDTO.Role
            };
            var adminUpdate = await _spaDbContext.Admins.FindAsync(AdminDTO.Email);
            if (adminUpdate is null) return false;


            adminUpdate.FirstName = AdminDTO.FirstName;
            adminUpdate.LastName = AdminDTO.LastName;
            adminUpdate.Email = AdminDTO.Email;
            adminUpdate.Role = AdminDTO.Role;
            adminUpdate.Phone = AdminDTO.Phone;
            adminUpdate.DateOfBirth = AdminDTO.DateOfBirth;
            adminUpdate.Gender = AdminDTO.Gender;

            var passwordHasher = new PasswordHasher<Admin>();
            adminUpdate.Password = passwordHasher.HashPassword(adminUpdate, AdminDTO.Password);
            _spaDbContext.Admins.Update(adminUpdate);
            return true;
        }
        public async Task<bool> UpdateEmployee(Employee EmpDTO)
        {
            if (EmpDTO is null) return false;

            var newUpdate = new Employee
            {
                //EmployeeID = EmpDTO.EmployeeID,
                FirstName = EmpDTO.FirstName,
                LastName = EmpDTO.LastName,
                Email = EmpDTO.Email,
                Password = EmpDTO.Password,
                Phone = EmpDTO.Phone,
                DateOfBirth = EmpDTO.DateOfBirth,
                Gender = EmpDTO.Gender,
                HireDate = EmpDTO.HireDate,
                JobTypeID = EmpDTO.JobTypeID,
                BranchID = EmpDTO.BranchID,            
                //EmployeeCode = EmpDTO.EmployeeCode,

            };
            var empUpdate = await _spaDbContext.Employees.FindAsync(EmpDTO.Email);
            if (empUpdate is null) return false;


            empUpdate.FirstName = EmpDTO.FirstName;
            empUpdate.LastName = EmpDTO.LastName;
            empUpdate.Email = EmpDTO.Email;

            empUpdate.Phone = EmpDTO.Phone;
            empUpdate.DateOfBirth = EmpDTO.DateOfBirth;
            empUpdate.Gender = EmpDTO.Gender;
            empUpdate.HireDate = EmpDTO.HireDate;
            empUpdate.JobType = EmpDTO.JobType;
            empUpdate.BranchID = EmpDTO.BranchID;

            var passwordHasher = new PasswordHasher<Employee>();
            empUpdate.Password = passwordHasher.HashPassword(empUpdate, EmpDTO.Password);
            _spaDbContext.Employees.Update(empUpdate);
            return true;
        }

        public async Task<Admin> GetLastAdminAsync()
        {
            try
            {
                return await _spaDbContext.Admins.OrderByDescending(a => a.AdminCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Employee> GetLastEmployeeAsync()
        {
            try
            {
                return await _spaDbContext.Employees.OrderByDescending(e => e.EmployeeCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}