using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Authentication;
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
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly SpaDbContext _spaDbContext;

        public AuthenticationController(IUserService userService, IMapper mapper, IMediator mediator, IWebHostEnvironment env, SpaDbContext spaDbContext)
        {
            _userService = userService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
            _spaDbContext = spaDbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var command = new RegisterCommand
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,                   
                    Email = userDto.Email,
                    Password = userDto.Password,
                    Role = userDto.Role,
                    PhoneNumber=userDto.Phone,
                    Gender=userDto.Gender,
                    DateOfBirth = userDto.DateOfBirth,
                    HireDate = DateTime.Now.Date,
                    JobTypeID =userDto.JobTypeID,
                    BranchID=userDto.BranchID,
    };
                var id = await _mediator.Send(command);
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
        }
        [HttpPost("CreateUserForEmployee")]
        public async Task<IActionResult> CreateUserForEmployee([FromBody] UserForEmployeeDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var command = new CreateUserForEmployeeCommand
                {
                    Email = userDto.Email,
                    Password = userDto.Password
                };
                var id = await _mediator.Send(command);
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
        }


        [HttpPost("login")]
        public async Task<AuthenticationResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return new AuthenticationResult(null,"Empty");
            }
            try
            {
                var command = new LoginCommand
                {
                    loginDTO = loginDto
                    //loginDTO = loginDto
                };
                var id = await _mediator.Send(command);
                //  return Ok(true);
                return id;
            }
            catch (DuplicateException ex)
            {
                return new AuthenticationResult(null, "Error");
            }
            catch (Exception ex)
            {
                return new AuthenticationResult(null, "Error");
            }
        }
    }
}
