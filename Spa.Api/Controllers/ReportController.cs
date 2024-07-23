using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public ReportController(IBillService billService)
        {
            _billService = billService;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        }
        [HttpGet("getdetail")]
        public async Task<ActionResult> GetRevenueReportByBranch(long idBrand, DateTime fromDate, DateTime toDate)
        {
            var billLineByDate = await _billService.GetRevenueReport(idBrand, fromDate, toDate);
            return new JsonResult(billLineByDate, _jsonSerializerOptions);
        }
        [HttpGet("getbyday")]
        public async Task<ActionResult> GetRevenueReportByDay(long idBrand, DateTime fromDate, DateTime toDate)
        {
            var billLineByDate = await _billService.GetRevenueReportByDay(idBrand, fromDate, toDate);
            return new JsonResult(billLineByDate, _jsonSerializerOptions);
        }
    }
}
