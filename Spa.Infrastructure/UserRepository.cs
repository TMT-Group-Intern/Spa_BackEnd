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
using Spa.Domain.Authentication;

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
        public async Task<Admin> GetAdminByEmail(string email)
        {
            var admin = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == email);
            //await _spaDbContext.SaveChangesAsync();
            return admin;
        }
        public async Task<Employee> GetEmpByEmail(string email)
        {
            var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(a => a.Email == email);
            //await _spaDbContext.SaveChangesAsync();
            return emp;
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
        }

        //
        //Login
        public async Task<string> LoginAccount(string Email, string Password)
        {
            if (Email is null || Password is null)
                return "Empty";

            var getUser = await _userManager.FindByEmailAsync(Email);
            if (getUser is null)
                return "User not exist";
            bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, Password);
            if (!checkUserPasswords)
                return null;
            string token = await GenerateToken(getUser.Code, (getUser.FirstName + " " + getUser.LastName), getUser.Email, getUser.Role);
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
        public async Task<string> GenerateToken(string Id, string Name, string Email, string Role)
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
            if (user.Role == "Admin")
            {
                var admin = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == Email);
                _spaDbContext.Admins.Remove(admin);
                _spaDbContext.SaveChanges();
                await _userManager.DeleteAsync(user);
                return true;
            }
            else
            {
                var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == Email);
                _spaDbContext.Employees.Remove(emp);
                _spaDbContext.SaveChanges();
                await _userManager.DeleteAsync(user);
                return true;
            }

        }

        //
        //Update
        public async Task<bool> UpdateUser(User UserDTO)
        {
            var newUpdate = new User
            {
                FirstName = UserDTO.FirstName,
                LastName = UserDTO.LastName,
                Email = UserDTO.Email,
                PasswordHash = UserDTO.PasswordHash,
                Role = UserDTO.Role,
                PhoneNumber = UserDTO.PhoneNumber,               
            };
            var userUpdate = await _userManager.FindByEmailAsync(UserDTO.Email);
            if (userUpdate is null) return false;
            userUpdate.FirstName = newUpdate.FirstName;
            userUpdate.LastName = newUpdate.LastName;
            //userUpdate.Email = newUpdate.Email;
            userUpdate.Role = newUpdate.Role;
            userUpdate.PhoneNumber = newUpdate.PhoneNumber;

            //user.PasswordHash = updateUserDTO.Password;
            var passwordHasher = new PasswordHasher<User>();
            userUpdate.PasswordHash = passwordHasher.HashPassword(userUpdate, newUpdate.PasswordHash);
            var updateUserResult = await _userManager.UpdateAsync(userUpdate);
            if (!updateUserResult.Succeeded)
                return false;
            return true;
        }

        public async Task<bool> UpdateAdmin(Admin AdminDTO)
        {
            var newUpdate = new Admin
            {
                FirstName = AdminDTO.FirstName,
                LastName = AdminDTO.LastName,
                Email = AdminDTO.Email,
                Password = AdminDTO.Password,
                Phone = AdminDTO.Phone,
                DateOfBirth = AdminDTO.DateOfBirth,
                Gender = AdminDTO.Gender,
                //Role = AdminDTO.Role,
                
            };
            var adminUpdate = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == newUpdate.Email);
            if (adminUpdate is null) return false;
            {
                adminUpdate.FirstName = newUpdate.FirstName;
                adminUpdate.LastName = newUpdate.LastName;
                adminUpdate.Email = newUpdate.Email;
                //adminUpdate.Role = newUpdate.Role;
                adminUpdate.Phone = newUpdate.Phone;
                adminUpdate.DateOfBirth = newUpdate.DateOfBirth;
                adminUpdate.Gender = newUpdate.Gender;
            }
            var passwordHasher = new PasswordHasher<Admin>();
            adminUpdate.Password = passwordHasher.HashPassword(adminUpdate, newUpdate.Password);
            _spaDbContext.Admins.Update(adminUpdate);
            _spaDbContext.SaveChanges();
            return true;
        }
        public async Task<bool> UpdateEmployee(Employee EmpDTO)
        {
            var newUpdate = new Employee
            {
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

            };
            var empUpdate = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == newUpdate.Email);
            if (empUpdate is null) return false;
            {
                empUpdate.FirstName = newUpdate.FirstName;
                empUpdate.LastName = newUpdate.LastName;
                empUpdate.Email = newUpdate.Email;
                empUpdate.Phone = newUpdate.Phone;
                empUpdate.DateOfBirth = newUpdate.DateOfBirth;
                empUpdate.Gender = newUpdate.Gender;
                empUpdate.HireDate = newUpdate.HireDate;
                empUpdate.JobType = newUpdate.JobType;
                empUpdate.BranchID = newUpdate.BranchID;
            }
            var passwordHasher = new PasswordHasher<Employee>();
            empUpdate.Password = passwordHasher.HashPassword(empUpdate, newUpdate.Password);
            _spaDbContext.Employees.Update(empUpdate);
            _spaDbContext.SaveChanges();
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