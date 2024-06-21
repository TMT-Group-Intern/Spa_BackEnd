using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Data;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator;
        private readonly IAppointmentService _appointmentService;

        public PaymentController(IPaymentService paymentService, IMediator mediator, IAppointmentService appointmentService)
        {
            _paymentService = paymentService;
            _mediator = mediator;
            _appointmentService = appointmentService;
        }

        [HttpGet("/GetRevenueToday")]
        public async Task<ActionResult> GetPaymentByDay()
        {
            var revenue = await _paymentService.GetRevenueToday();
            return Ok(new { revenue = revenue });
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(long Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Appointment app = _appointmentService.GetAppointmentByIdAsync(Id);
                var command = new CreatePaymentCommand
                {
                    AppointmentID = Id,
                    Amount = app.Total,
                    CustomerID = app.CustomerID,
                    CreatedAt = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    Status = "Completed",
                    PaymentMethod = "Cash"
                };
                var a = await _mediator.Send(command);
                return Ok(new { a });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("/GetPaymentByBranch")]
        public async Task<ActionResult> GetPaymentByBranch(long branchID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var list = await _paymentService.GetAllPaymentsByBranch(branchID);
                return Ok(list);
            }
            catch (ErrorMessage ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ExportExel")]
        public async Task<FileResult> ExportExelPayment(long branchID)
        {
            var list = await _paymentService.GetAllPaymentsByBranch(branchID);
            var filename = $"Payment_At_Branch_{branchID}.xlsx";
            return GenerateExel(filename, list);
        }

        private FileResult GenerateExel(string filename, List<Payment> payments)
        {
            DataTable dataTable = new DataTable("Payment");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn ("Customer Code"),
                new DataColumn ("Customer Name"),
                new DataColumn ("Payment Date"),
                new DataColumn ("Total"),
                new DataColumn ("Payment Method"),
                new DataColumn ("Create At"),
                new DataColumn ("Note")
            });

            foreach( var payment in payments)
            {
                dataTable.Rows.Add(payment.Customer.CustomerCode, payment.Customer.FirstName +" "+ payment.Customer.LastName, payment.PaymentDate, payment.Amount, payment.PaymentMethod, payment.CreatedAt, payment.Notes == null? "": payment.Notes);
            }

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                      filename);
                }
            }
        }
    }
}
