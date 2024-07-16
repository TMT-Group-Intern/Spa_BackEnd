using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IMediator _mediator;
        private readonly IAppointmentService _appointmentService;

        public BillController(IAppointmentService appointmentService, IBillService billService, IMediator mediator) {
            _billService = billService;
            _mediator = mediator;
            _appointmentService = appointmentService;
        }


        [HttpPost]
        public async Task<ActionResult> CreateBill(CreateBillDTO createBillDTO) //id appointment
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
               // Appointment app = _appointmentService.GetAppointmentByIdAsync(Id);
                
                    var command = new CreateBillCommand
                    {
                        AppointmentID = createBillDTO.AppointmentID,
                        CustomerID = createBillDTO.CustomerID,
                        Date = DateTime.Now,
                        BillStatus = "Đang điều trị",                                  
                        Doctor = createBillDTO.Doctor,
                        TechnicalStaff = createBillDTO.TechnicalStaff,
                        TotalAmount = createBillDTO.TotalAmount,
                        AmountInvoiced = createBillDTO.AmountInvoiced ,
                        AmountResidual = createBillDTO.AmountResidual , //số tiền còn lại (chưa trả)
                     //   BillItems = createBillDTO.BillItems
                    };
                    var item = await _mediator.Send(command);
                    return Ok(new { item });
                
             
            }
            catch (ErrorMessage ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
