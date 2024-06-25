using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Infrastructure;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationControllerTest : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly SpaDbContext _spaDbContext;

        public AuthenticationControllerTest(IUserService userService, IMapper mapper, IMediator mediator, IWebHostEnvironment env, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
            _spaDbContext = spaDbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Code = userDto.Code,
                PasswordHash = userDto.Password,
                Role = userDto.Role,
            };
            await _userService.CreateUser(user);
            if (user.Role == "Admin")
            {
                var admin = new Admin
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    AdminCode = user.Code,
                    Password = user.PasswordHash,
                    Role = user.Role,
                    //Id= user.Id
                };
                await _spaDbContext.Admins.AddAsync(admin);
                await _spaDbContext.SaveChangesAsync();
            }
            else
            {
                var emp = new Employee
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmployeeCode = user.Code,
                    Password = user.PasswordHash,

                    //Id = user.Id
                };
                await _spaDbContext.Employees.AddAsync(emp);
                await _spaDbContext.SaveChangesAsync();
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var command = new LoginCommand
                {
                    loginDTO = loginDto
                };
                var id = await _mediator.Send(command);
                //  return Ok(true);
                return Ok(id);
            }
            catch (DuplicateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

/*
            var user = await _userService.LoginAccount(loginDto.Email, loginDto.Password);
            if (user is null) return BadRequest("Invalid email or password.");
            return Ok(user);*/
        }
        [HttpPost("register2")]
        public async Task<IActionResult> Register2(UserDTO userDTO)
        {
            if (userDTO.Role == "Admin")
            {
                var admin = new Admin
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    AdminCode = userDTO.Code,
                    //Password = userDTO.Password,
                    Role = userDTO.Role,
                };
                await _spaDbContext.Admins.AddAsync(admin);
                await _spaDbContext.SaveChangesAsync();
                var user = new User
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    Code = admin.AdminCode,
                    PasswordHash = userDTO.Password,
                    Role = admin.Role,
                };
                await _userService.CreateUser(user);
                return Ok(admin);
            }
            else
            {
                var emp = new Employee
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    EmployeeCode = userDTO.Code,
                    //Password = userDTO.Password,
   
                };
                await _spaDbContext.Employees.AddAsync(emp);
                await _spaDbContext.SaveChangesAsync();
                var user = new User
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    Code = emp.EmployeeCode,
                    PasswordHash = userDTO.Password
         
                };
                await _userService.CreateUser(user);
                return Ok(emp);
            }
        }
    }
}
