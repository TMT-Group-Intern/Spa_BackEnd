using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };    
            var app = await _service.GetAllAppoment();
            Appointment a = new Appointment
            {
                AppointmentDate = app.Select(x => x.AppointmentDate).FirstOrDefault()
            };
            var b = a.AppointmentDate;

            var json = JsonSerializer.Serialize(app, options);
            if (app == null)
            {
                NotFound();
            }
            return Ok(json);
        }
    }
}
