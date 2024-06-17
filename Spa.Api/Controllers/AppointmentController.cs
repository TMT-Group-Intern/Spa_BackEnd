using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AppointmentController(IAppointmentService service, IMediator mediator, IMapper mapper)
        {
            _service = service;
            _mediator = mediator;
            _mapper = mapper;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        [HttpGet]
        public ActionResult GetAll()
        {

            // JsonResult result = new JsonResult(0);
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Customer, CustomerDTO>();
            //});
            //   var _mapper = config.CreateMapper();
            var app = _service.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
              //  EmployeeID = a.EmployeeID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
               
            });;
            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(app, _jsonSerializerOptions);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateAppointmentDTO appointmentCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateAppointmentCommand
                {
                    CustomerID = appointmentCreateDto.CustomerID,
                    AppointmentDate = appointmentCreateDto.AppointmentDate,
                    BranchID = appointmentCreateDto.BranchID,
                    EmployeeID = appointmentCreateDto.EmployeeID,
                    Status = appointmentCreateDto.Status,
                    Total = appointmentCreateDto.Total,
                    ServiceID = appointmentCreateDto.ServiceID,
                };
                var id = await _mediator.Send(command);

                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAppointmentById(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appByid = _mapper.Map<AppointmentDTO>(_service.GetAppointmentByIdAsync(id));
            if (appByid == null)
            {
                return NotFound();
            }

            return new JsonResult(appByid, _jsonSerializerOptions); ;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> updateAppointmentWithoutService(long id, [FromBody] UpdateAppointmentWithoutServiceDTO updateAppointmentWithoutServiceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Appointment app = new Appointment
            {
                AppointmentDate = updateAppointmentWithoutServiceDTO.AppointmentDate,
                BranchID = updateAppointmentWithoutServiceDTO.BranchID,
                CustomerID = updateAppointmentWithoutServiceDTO.CustomerID,
                Status = updateAppointmentWithoutServiceDTO.Status,
                Total = updateAppointmentWithoutServiceDTO.Total,   
                Assignments = updateAppointmentWithoutServiceDTO.Assignments.Select(a => new Assignment
                {
                    AppointmentID = id,
                    EmployerID = a.EmployerID,
                }).ToList()
            };
            await _service.UpdateAppointmentWithoutService(id, app);

            return Ok(new { id });
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult> DeleteAppointmentById(long id)
        {
         try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _service.DeleteAppointment(id))
                {
                    return Ok(new { id });
                }
            }
            catch (ErrorMessage ex)
            {
                 return BadRequest(new { Message = ex.Message }); ;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
            return NotFound();
        }
    }
}
