using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IMediator _mediator;

        public CustomersController(ICustomerService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var customersFromService = _service.GetAllCustomer();

            var customerDTO = customersFromService.Select(c => new CustomerDTO
            {
                CustomerID = c.CustomerID,
                CustomerCode = c.CustomerCode,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DateOfBirth = c.DateOfBirth,
                Email = c.Email,
                Gender = c.Gender,
                Phone = c.Phone
            });
            return Ok(new { item = customerDTO });
        }

        [HttpGet("{id}")]
        public ActionResult GetCusomerById(long id)
        {
            if (_service.isExistCustomer(id))
            {
                var getByCusByID = _service.GetCustomerById(id);

                CustomerDTO customerDTO = new CustomerDTO
                {
                    CustomerID = getByCusByID.CustomerID,
                    CustomerCode = getByCusByID.CustomerCode,
                    DateOfBirth = getByCusByID.DateOfBirth,
                    Email = getByCusByID.Email,
                    FirstName = getByCusByID.FirstName,
                    LastName = getByCusByID.LastName,
                    Gender = getByCusByID.Gender,
                    Phone = getByCusByID.Phone
                    
                };
                return Ok(new { customerDTO });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDto)
        {
            try
            {
                var command = new CreateCustomerCommand
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    DateOfBirth = customerDto.DateOfBirth,
                    Gender = customerDto.Gender,
                    Phone = customerDto.Phone
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
        }


        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(long customerId, [FromBody] CustomerDTO customerDto)
        {
            try
            {
                if (customerDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_service.isExistCustomer(customerId))
                {
                    return NotFound();
                }
                Customer customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    DateOfBirth = customerDto.DateOfBirth,
                    Gender = customerDto.Gender,
                    Phone = customerDto.Phone
                };
                await _service.UpdateCustomer(customerId, customer);
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

        [HttpDelete("{customerId}")]
        public async Task<ActionResult> DeactivateCustomer(long customerId)
        {
            try
            {
                if (_service.isExistCustomer(customerId))
                {
                    await _service.DeleteCustomer(customerId);
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
