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
        public ActionResult<IEnumerable<Customer>> Get()
        {
            var customersFromService = _service.GetAllCustomer();
            return Ok(customersFromService);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDto)
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
            catch (DuplicatePhoneNumberException ex)
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
