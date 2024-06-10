using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
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
        public ActionResult<IEnumerable<ServiceEntity>> Get()
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
            return Ok(serviceDTO);
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
            var command = new CreateServiceCommand
            {
               serviceDTO = serviceDto
            };
            var id = await _mediator.Send(command);
            //  return Ok(true);
            return Ok(id);
        }

        // PUT api/<ServicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
