using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserService userService, IMapper mapper, IMediator mediator, IWebHostEnvironment env)
        {
            _userService = userService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {

            if (_userService.isExistUser(email))
            {
                var getUserByEmail = _userService.GetUserByEmail(email);

                UserDTO userDTO = new UserDTO
                {
                    Id = getUserByEmail.Result.Id,
                    FirstName = getUserByEmail.Result.FirstName,
                    LastName = getUserByEmail.Result.LastName,
                    Email = getUserByEmail.Result.Email,
                    Code = getUserByEmail.Result.Code,
                    Role = getUserByEmail.Result.Role,
                };
                return Ok(new { userDTO });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UpdateDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_userService.isExistUser(email))
                {
                    return NotFound();
                }
                User user = new User
                {
                    FirstName = updateDto.FirstName,
                    LastName = updateDto.LastName,
                    PasswordHash=updateDto.Password,
                    Role = updateDto.Role,
                };
                await _userService.UpdateUser(user);
                if(user.Role == "Admin")
                {
                    Admin admin = new Admin
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.PasswordHash,
                        Role = user.Role,
                        DateOfBirth = updateDto.DateOfBirth,
                        Phone = updateDto.Phone,
                        Gender = updateDto.Gender,  
                    };
                 await _userService.UpdateAdmin(admin);   
                }
                else
                {
                    Employee emp = new Employee
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.PasswordHash,
                        Role = user.Role,
                        Gender= updateDto.Gender,
                        HireDate = updateDto.HireDate,
                        Phone = updateDto.Phone,
                        BranchID = updateDto.BranchID,
                        DateOfBirth= updateDto.DateOfBirth,
                        JobTypeID = updateDto.JobTypeID,
                        
                    };
                    await _userService.UpdateEmployee(emp);
                }
                return Ok(true);
            }
            catch (DuplicateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteUser(string email)
        {
            try
            {
                if (_userService.isExistUser(email))
                {
                    await _userService.DeleteUser(email);
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
