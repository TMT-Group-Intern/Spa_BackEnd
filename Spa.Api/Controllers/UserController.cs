using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
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

        [HttpGet("allUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpGet("getUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
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
        [HttpGet("getUserByAdmin")]
        public async Task<IActionResult> GetAdminByEmail(string email)
        {

            var getAdminByEmail = _userService.GetAdminByEmail(email);

            AdminDTO adminDTO = new AdminDTO
            {
                AdminID = getAdminByEmail.Result.AdminID,
                FirstName = getAdminByEmail.Result.FirstName,
                LastName = getAdminByEmail.Result.LastName,
                Email = getAdminByEmail.Result.Email,
                AdminCode = getAdminByEmail.Result.AdminCode,
                Role = getAdminByEmail.Result.Role,
                DateOfBirth=getAdminByEmail.Result.DateOfBirth,
                Gender = getAdminByEmail.Result.Gender,
                Password = getAdminByEmail.Result.Password,
                Phone = getAdminByEmail.Result.Phone
                
            };
            return Ok(new { adminDTO });
        }
        [HttpGet("getUserByEmployee")]
        public async Task<IActionResult> GetEmpByEmail(string email)
        {

            var getEmpByEmail = _userService.GetEmpByEmail(email);

            EmployeeDTO empDTO = new EmployeeDTO
            {
                EmployeeID = getEmpByEmail.Result.EmployeeID,
                FirstName = getEmpByEmail.Result.FirstName,
                LastName = getEmpByEmail.Result.LastName,
                Email = getEmpByEmail.Result.Email

            };
            return Ok(new { empDTO });
        }
        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UpdateDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User user = new User
                {
                    FirstName = updateDto.FirstName,
                    LastName = updateDto.LastName,
                    PasswordHash=updateDto.Password,
                    Role = updateDto.Role,
                    PhoneNumber=updateDto.Phone,
                    Email = email,
                };
                //await _userService.UpdateUser(user);
                if (user.Role == "Admin")
                {
                    Admin admin = new Admin
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.PasswordHash,
                        //Role = user.Role,
                        DateOfBirth = updateDto.DateOfBirth,
                        Phone = updateDto.Phone,
                        Gender = updateDto.Gender,
                        Email = user.Email
                    };
                    await _userService.UpdateAdmin(admin);
                    await _userService.UpdateUser(user);
                    return Ok(true);
                }
                else
                {
                    Employee emp = new Employee
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = user.PasswordHash,
                        Gender = updateDto.Gender,
                        HireDate = updateDto.HireDate,
                        Phone = updateDto.Phone,
                        BranchID = updateDto.BranchID,
                        DateOfBirth = updateDto.DateOfBirth,
                        JobTypeID = updateDto.JobTypeID,
                        Email = user.Email
                    };
                    await _userService.UpdateEmployee(emp);
                    await _userService.UpdateUser(user);
                    return Ok(true);
                }
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
        [HttpDelete("deleteUser")]
        public async Task<ActionResult> DeleteUser(string email)
        {
            try
            {
                    await _userService.DeleteUser(email);
                    return Ok(true);
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
