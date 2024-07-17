using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IMediator _mediator;
        private readonly IAppointmentService _appointmentService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BillController(IAppointmentService appointmentService, IBillService billService, IMediator mediator)
        {
            _billService = billService;
            _mediator = mediator;
            _appointmentService = appointmentService;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }


        [HttpPost]
        public async Task<ActionResult> CreateBill(CreateBillDTO createBillDTO)
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
                    AmountInvoiced = createBillDTO.AmountInvoiced,
                    AmountResidual = createBillDTO.TotalAmount,
               
                   
                    //số tiền còn lại (chưa trả)
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

        [HttpGet]
        public async Task<ActionResult> GetAllBillAsync()
        {
            var billLine = await _billService.GetAllBillAsync();
            return new JsonResult(billLine);
        }

        [HttpGet("getbillbycustomer")]
        public async Task<ActionResult> GetAllBillByCustomerAsync(long cusId)
        {
            var billLine = await _billService.GetAllBillByCustomerAsync(cusId);
            return new JsonResult(billLine);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBillByIdAsync(long id)
        {
            try
            {
                var bill = await _billService.GetBillByIdAsync(id);
                if (bill != null)
                {
                    return new JsonResult(bill, _jsonSerializerOptions);
                }
                return NotFound();
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBill(long id, Bill bill)
        {
            try
            {
                var checkExistBill = await _billService.GetBillByIdAsync(id);
                if (checkExistBill != null)
                {
                    var billWasUpdate = await _billService.UpdateBill(id, bill);
                    return new JsonResult(billWasUpdate, _jsonSerializerOptions);
                }
                return NotFound();
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
