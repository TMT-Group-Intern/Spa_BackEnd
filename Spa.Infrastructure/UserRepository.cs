using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<string> GetUserBoolByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null) { 
                return "true";
            }
            return "false";
        }
        public async Task<Admin> GetAdminByEmail(string email)
        {
            var admin = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == email);
            return admin;
        }
        public async Task<Employee> GetEmpByEmail(string email)
        {
            var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
            return emp;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users is null)
            {
                return null;
            }

            var userDTOs = users.Select(user => new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Code = user.Code,
                IsActiveAcount = user.IsActiveAcount,
            }).OrderBy(u => u.Code).ToList();

            return userDTOs;
        }
        public async Task<List<Employee>> GetAllAdminsAndEmployees()
        {
          var user = await _spaDbContext.Employees.OrderBy(u => u.EmployeeCode).Include(j => j.JobType).ToListAsync();

            return user;
        }

        public async Task<int> GetAllItemProduct()
        {
            return await _userManager.Users.CountAsync();
        }
        public async Task<IEnumerable<User>> GetByPages(int pageNumber, int pageSize)
        {
            return await _userManager.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<List<Employee>> GetAllEmployee()
        {
            var emps = await _spaDbContext.Employees
                .ToListAsync();
            if (emps is null)
            {
                return null;
            }
                var empDTOs = emps.Select(emp =>new Employee
                {
                    EmployeeCode = emp.EmployeeCode,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    Phone = emp.Phone,
                    Gender = emp.Gender,
                    BranchID = emp.BranchID,
                    DateOfBirth = emp.DateOfBirth,
                    HireDate = emp.HireDate,                    
                    Assignments = emp.Assignments,
                    JobTypeID = emp.JobTypeID,
                    IsActive = emp.IsActive,
                }).OrderBy(e=>e.EmployeeCode).ToList();
            return empDTOs;
        }

        public async Task<List<Employee>> GetEmployeeByBranchAndJob(long branchID, long jobTypeID)
        {
            var emps = await _spaDbContext.Employees
                .Where(e => e.BranchID == branchID && e.JobTypeID == jobTypeID && e.IsActive)
                .ToListAsync();
            if (emps is null || !emps.Any())
            {
                return null;
            }

            var empDTOs = emps.Select(emp => new Employee
            {
                EmployeeID = emp.EmployeeID,
                EmployeeCode = emp.EmployeeCode,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                Phone = emp.Phone,
                Gender = emp.Gender,
                BranchID = emp.BranchID,
                DateOfBirth = emp.DateOfBirth,
                HireDate = emp.HireDate,
                Assignments = emp.Assignments,
                JobTypeID = emp.JobTypeID,
                IsActive = emp.IsActive,
            }).OrderBy(e => e.EmployeeCode).ToList();
            return empDTOs;
        }
        public async Task<List<Admin>> GetAllAdmin()
        {
            var admins = await _spaDbContext.Admins.ToListAsync();
            if (admins is null)
            {
                return null;
            }

            var adminDTOs = admins.Select(admin => new Admin
            {
                AdminCode = admin.AdminCode,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Phone = admin.Phone,
                Gender = admin.Gender,
                DateOfBirth = admin.DateOfBirth,
                IsActive = admin.IsActive,
            }).OrderBy(a => a.AdminCode).ToList();

            return adminDTOs;
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
                AdminID = userDTO.AdminID,
                EmployeeID = userDTO.EmployeeID,
                IsActiveAcount = true,
            };
            var createUser = await _userManager.CreateAsync(newUser!, userDTO.PasswordHash);
            if (!createUser.Succeeded)
            {
                throw null;
            }
            return newUser;
        }
        public async Task<User> CreateUserForEmployee(string Email)
        {
            var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == Email);
            if (emp is null) return null;
            var newUser = new User()
            {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                PasswordHash = "Spa@12345",
                Code = emp.EmployeeCode,
                UserName = emp.Email,
                PhoneNumber = emp.Phone,
                EmployeeID = emp.EmployeeID,
                Id = emp.Id,
                IsActiveAcount = true
            };
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
            {
                return null;
            }
            var userRole = await _spaDbContext.JobTypes.FindAsync(emp.JobTypeID);
            newUser.Role = userRole.JobTypeName;
            var createUser = await _userManager.CreateAsync(newUser!, newUser.PasswordHash);
            if (!createUser.Succeeded)
            {
                return null;
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
                //expires: DateTime.Now.AddDays(1),
                expires: DateTime.MaxValue,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> DeleteUser(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if(user is null)
            {
                var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == Email);
                if (emp is null) return false;
                {
                    emp.IsActive = false;
                }
                _spaDbContext.Employees.Update(emp);
                _spaDbContext.SaveChanges();
                return true;
            }
            if (user.Role == "Admin")
            {
                var admin = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == Email);
                if (admin.Email is null) return false;
                {
                    admin.IsActive = false;
                }
                _spaDbContext.Admins.Update(admin);
                _spaDbContext.SaveChanges();

                var userUpdate = await _userManager.FindByEmailAsync(Email);
                if (userUpdate is null) return false;
                userUpdate.IsActiveAcount = false;
                var updateUserResult = await _userManager.UpdateAsync(userUpdate);
                if (!updateUserResult.Succeeded) return false;

                return true;
            }
            else
            {
                var emp = await _spaDbContext.Employees.FirstOrDefaultAsync(e => e.Email == Email);
                if (emp.Email is null) return false;
                {
                    emp.IsActive = false;
                }
                _spaDbContext.Employees.Update(emp);
                _spaDbContext.SaveChanges();

                var userUpdate = await _userManager.FindByEmailAsync(Email);
                if (userUpdate is null) return false;
                userUpdate.IsActiveAcount = false;
                var updateUserResult = await _userManager.UpdateAsync(userUpdate);
                if (!updateUserResult.Succeeded) return false;

                return true;
            }
        }

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
            userUpdate.Role = newUpdate.Role;
            userUpdate.PhoneNumber = newUpdate.PhoneNumber;

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
                Phone = AdminDTO.Phone,
                DateOfBirth = AdminDTO.DateOfBirth,
                Gender = AdminDTO.Gender,
                Role= "Admin",
            };
            var adminUpdate = await _spaDbContext.Admins.FirstOrDefaultAsync(a => a.Email == newUpdate.Email);
            if (adminUpdate is null) return false;
            {
                adminUpdate.FirstName = newUpdate.FirstName;
                adminUpdate.LastName = newUpdate.LastName;
                adminUpdate.Email = newUpdate.Email;
                adminUpdate.Phone = newUpdate.Phone;
                adminUpdate.DateOfBirth = newUpdate.DateOfBirth;
                adminUpdate.Gender = newUpdate.Gender;
                adminUpdate.Role = newUpdate.Role;
            }
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
                empUpdate.JobTypeID = newUpdate.JobTypeID;
                empUpdate.BranchID = newUpdate.BranchID;
            }
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