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
using Spa.Domain.Service;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AppointmentController(IAppointmentService appointmentService, IMediator mediator, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mediator = mediator;
            _mapper = mapper;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        [HttpGet]
        public ActionResult GetAll(long idBrand)
        {
            var app = _appointmentService.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault(),
            });
            var appByBrand = app.Where(e => e.BranchID == idBrand && e.AppointmentDate >= DateTime.Today);
            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(appByBrand, _jsonSerializerOptions);
        }

        [HttpGet("/GetAllByBranch")]
        public ActionResult GetAllByBranch(long idBrand)
        {
            var app = _appointmentService.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault(),
            });
            var appByBrand = app.Where(e => e.BranchID == idBrand && e.AppointmentDate >= DateTime.Today);
            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(appByBrand, _jsonSerializerOptions);
        }

        [HttpGet("/GetAppointmentByStatus")]
        public ActionResult GetAllByStatus(long idBrand, string status)
        {
            var app = _appointmentService.GetAllAppoinment().Select(a => new AppointmentDTO
            {
                AppointmentID = a.AppointmentID,
                BranchID = a.BranchID,
                CustomerID = a.CustomerID,
                Status = a.Status,
                Total = a.Total,
                AppointmentDate = a.AppointmentDate,
                Customer = _mapper.Map<CustomerDTO>(a.Customer),
                Doctor = a.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault(),
                TeachnicalStaff = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault(),
                EmployeeCode = a.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(e => e.Employees.EmployeeCode).FirstOrDefault()

            });
            var appByBrand = app.Where(e => e.BranchID == idBrand && e.Status == status && e.AppointmentDate >= DateTime.Today);
            if (app == null)
            {
                NotFound();
            }
            return new JsonResult(appByBrand, _jsonSerializerOptions);
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
            var getDoctorAndStaff = _appointmentService.GetAppointmentByIdAsync(id);
            var appByid = _mapper.Map<AppointmentDTO>(_appointmentService.GetAppointmentByIdAsync(id));
            AppointmentDTO appointmentDTO = new AppointmentDTO
            {
                TeachnicalStaff = getDoctorAndStaff.Assignments.Where(e => e.Employees.JobTypeID == 3).Select(n => n.Employees.FirstName + " " + n.Employees.LastName).FirstOrDefault(),
                Doctor = getDoctorAndStaff.Assignments.Where(e => e.Employees.JobTypeID == 2).Select(e => e.Employees.FirstName + " " + e.Employees.LastName).FirstOrDefault()
            };
             appByid.Doctor = appointmentDTO.Doctor;
             appByid.TeachnicalStaff =  appointmentDTO.TeachnicalStaff;
            if (appByid == null)
            {
                return NotFound();
            }

            return new JsonResult(appByid, _jsonSerializerOptions); ;
        }

        [HttpPut("updatestatus/{id}")]
        public async Task<ActionResult> updateStatus(long id, string status)
        {
            await _appointmentService.UpdateStatus(id, status);
            return Ok();
        }

        [HttpPut("assigntechnicalstaff")]
        public async Task<ActionResult> AssignTechnicalStaff(long idApp, long idEmploy)
        {
            await _appointmentService.AssignTechnicalStaff(idApp, idEmploy);
            return Ok(true);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> updateAppointmentWithoutService(long id, [FromBody] UpdateAppointmentWithoutServiceDTO updateAppointmentWithoutServiceDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Appointment app = new Appointment
                {
                    AppointmentDate = updateAppointmentWithoutServiceDTO.AppointmentDate,
                    //  BranchID = updateAppointmentWithoutServiceDTO.BranchID,
                    // CustomerID = updateAppointmentWithoutServiceDTO.CustomerID,
                    Status = updateAppointmentWithoutServiceDTO.Status,
                    Assignments = updateAppointmentWithoutServiceDTO.Assignments.Select(a => new Assignment
                    {
                        //   AppointmentID = id,
                        EmployerID = a.EmployerID,
                    }).ToList()
                };
                await _appointmentService.UpdateAppointmentWithoutService(id, app);

                return Ok(new { id });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        [HttpPut("api/UpdateAppointmentWithService/{id}/{status}")]
        public async Task<ActionResult> updateAppointmentWithService(long id, string status, [FromBody] List<long> serviceID, string? notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _appointmentService.UpdateAppointmentWithService(id, serviceID, status, notes);

            return Ok(new { id });
        }

        [HttpPut("Test/{id}")]
        public async Task<ActionResult> UpdateAppointment(long id, UpdateAppointmentDTO appointment)  //Update Appointment (RESTFUll)
        {
            ICollection<ChooseService>? chooseServices = new List<ChooseService>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if ( _appointmentService.GetAppointmentByIdAsync(id) != null)
            {
                if (appointment.ListServiceID != null)
                {
                    foreach (var item in appointment.ListServiceID)
                    {
                        {
                            chooseServices!.Add(new ChooseService { AppointmentID = id, ServiceID = item });
                        };
                    }
                }

                Appointment app = new Appointment
                {
                    AppointmentID = id,
                    AppointmentDate = appointment.AppointmentDate ,
                    BranchID = appointment.BranchID,
                    CustomerID = appointment.CustomerID,
                    Notes = appointment.Notes,
                    DiscountPercentage = appointment.DiscountPercentage,
                    Status = appointment.Status,
                    ChooseServices = chooseServices
                };
                await _appointmentService.UpdateAppointment(id, app);

                return Ok(new { id, appointment });
            }
            return NotFound();

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointmentById(long id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _appointmentService.DeleteAppointment(id))
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

        [HttpPut("UpdateDiscount")]
        public async Task<ActionResult> UpdateDiscount(long id, int perDiscount)
        {
            var a = await _appointmentService.UpdateDiscount(id, perDiscount);
            return Ok(a);
        }

    }

}
