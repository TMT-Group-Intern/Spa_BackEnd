using AutoMapper;
using MediatR;
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
                    HireDate = userDto.HireDate,
                    JobTypeID =userDto.JobTypeID,
                    BranchID=userDto.BranchID,
    };
                var id = await _mediator.Send(command);
                return Ok(new {status = id});
            }
            catch (DuplicateException ex1)
            {
                return Ok(new {});
            }
            catch (Exception ex2)
            {
                return Ok(new {});
            }
        }
        [HttpPost("CreateUserForEmployee")]
        public async Task<IActionResult> CreateUserForEmployee([FromBody] UserForEmployeeDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var command = new CreateUserForEmployeeCommand
                {
                    Email = userDTO.Email,
                };
                var id = await _mediator.Send(command);
                return Ok(new { status = id });
            }
            catch (DuplicateException ex)
            {
                return  Ok(new {});
            }
            catch (Exception ex)
            {
                return  Ok(new {});
            }
        }


        [HttpPost("login")]
        public async Task<AuthenticationResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return new AuthenticationResult(false,"Empty",null,null);
            }
            try
            {
                var command = new LoginCommand
                {
                    loginDTO = loginDto
                };
                var id = await _mediator.Send(command);
                return id;
            }
            catch (DuplicateException ex)
            {
                return new AuthenticationResult(false, "Error",null,null);
            }
            catch (Exception ex)
            {
                return new AuthenticationResult(false, "Error",null,null);
            }
        }
    }
}
