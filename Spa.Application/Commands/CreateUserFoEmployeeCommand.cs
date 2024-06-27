using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateUserForEmployeeCommand : IRequest<string>
    { 
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class CreateUserForEmployeeCommandHandler : IRequestHandler<CreateUserForEmployeeCommand, string>
    {
        private readonly IUserService _userService;
        private readonly SpaDbContext _spaDbContext;

        public CreateUserForEmployeeCommandHandler(IUserService userService, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _spaDbContext = spaDbContext;
        }
        public async Task<string> Handle(CreateUserForEmployeeCommand request, CancellationToken cancellationToken)
        {
            var emp = await _userService.GetEmpByEmail(request.Email);
            if (emp is null)
            {
                throw new Exception("Employee not exist");
            }
            var user = new UserForEmployeeDTO
            {
                Email = emp.Email,
                Password=request.Password
            };
            var newUser = await _userService.CreateUserForEmployee(user.Email, user.Password);
            if (newUser is null)
            {
                throw new Exception("Error!");
            }
            return "Create Success!";
        }
    }
}
