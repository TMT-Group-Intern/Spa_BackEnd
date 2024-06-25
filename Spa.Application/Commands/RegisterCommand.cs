using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Domain.Service;
using Spa.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class RegisterCommand : IRequest<string>
    {
        public string FirstName { get; set; }
        public string LastName {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender {  get; set; }
        public long? JobTypeID { get; set; }
        public long? BranchID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }= DateTime.Now.Date;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IUserService _userService;
        private readonly SpaDbContext _spaDbContext;

        public RegisterCommandHandler(IUserService userService, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _spaDbContext = spaDbContext;
        }
        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = request.Password,
                Email = request.Email,
                Role = request.Role,
                PhoneNumber = request.PhoneNumber,
                //Code = request.userDTO.Code,
            };
            var newUser = await _userService.CreateUser(user);
            if (newUser is null)
            {
                throw new Exception("Error!");
            }
            if (user.Role == "Admin")
            {
                var admin = new Admin
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    AdminCode = newUser.Code,
                    Password = newUser.PasswordHash,
                    Role = newUser.Role,
                    Phone= newUser.PhoneNumber,
                    Id= newUser.Id.ToString(),
                    DateOfBirth= request.DateOfBirth,
                    Gender= request.Gender,
                };
                //await _spaDbContext.Admins.AddAsync(admin);
                //await _spaDbContext.SaveChangesAsync();
                await _userService.CreateAdmin(admin);
                return "Create Success!";
            }
            else
            {
                var emp = new Employee
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    EmployeeCode = newUser.Code,
                    Password = newUser.PasswordHash,
                    Phone = newUser.PhoneNumber,
                    Id = newUser.Id.ToString(),
                    DateOfBirth= request.DateOfBirth,
                    HireDate = request.HireDate,
                    Gender = request.Gender,
                    JobTypeID=request.JobTypeID,
                    BranchID=request.BranchID
                };
                /*await _spaDbContext.Employees.AddAsync(emp);
                await _spaDbContext.SaveChangesAsync();*/
                await _userService.CreateEmployee(emp);
                return "Create Success!";
            }
        }
    }
}