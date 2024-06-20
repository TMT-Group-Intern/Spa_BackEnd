using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Domain.Service;

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
                    CreatedAt = DateTime.UtcNow,
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
    }
}
