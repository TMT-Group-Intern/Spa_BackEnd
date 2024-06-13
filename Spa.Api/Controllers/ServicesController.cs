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
    public class ServicesController : ControllerBase
    {

        private readonly IServiceService _service;
        private readonly IMediator _mediator;

        public ServicesController(IServiceService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        // GET: api/<ServicesController>
        [HttpGet]
        public ActionResult GetAll()
        {
            var allService = _service.GetAllService();
            var serviceDTO = allService.Select(s => new ServiceDTO
            {
                ServiceID = s.ServiceID,
                Description = s.Description,
                Price = s.Price,
                ServiceCode = s.ServiceCode,
                ServiceName = s.ServiceName,

            }).ToList();
            return Ok(new { items = serviceDTO });
        }

        // GET api/<ServicesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServicesController>
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceDTO serviceDto)
        {
            try
            {
                var command = new CreateServiceCommand
                {
                    serviceDTO = serviceDto
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

        // PUT api/<ServicesController>/5
        [HttpPut("{serviceId}")]
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] ServiceDTO serviceDto)
        {
            try
            {
                if (serviceDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_service.isExistService(serviceId))
                {
                    return NotFound();
                }
                ServiceEntity service = new ServiceEntity
                {
                    ServiceName = serviceDto.ServiceName,
                    Description = serviceDto.Description,
                    Price = serviceDto.Price,
                };
                await _service.UpdateService(serviceId, service);
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

        // DELETE api/<ServicesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id)
        {
            try
            {
                if (_service.isExistService(id))
                {
                    await _service.DeleteService(id);
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
