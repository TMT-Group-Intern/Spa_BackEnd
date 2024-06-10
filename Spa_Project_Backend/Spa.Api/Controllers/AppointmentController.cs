using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application;
using Spa.Domain.Entities;
using Spa.Domain.IService;

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
        public ActionResult<IEnumerable<Appointment>> Get()
        {
            var app = _service.GetAllAppoment();
            if(app == null)
            {
                NotFound();
            }
            return Ok(app);
        }
    }
}
