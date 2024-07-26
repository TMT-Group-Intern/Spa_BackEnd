﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Text.Json.Serialization;
using System.Text.Json;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;

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
        [HasPermission(SetPermission.GetRevenueReportByBranch)]

        public async Task<ActionResult> GetRevenueReportByBranch(long idBrand, DateTime fromDate, DateTime toDate)
        {
            var billLineByDate = await _billService.GetRevenueReport(idBrand, fromDate, toDate);
            return new JsonResult(billLineByDate, _jsonSerializerOptions);
        }
        [HttpGet("getbyday")]
        [HasPermission(SetPermission.GetRevenueReportByDay)]
        public async Task<ActionResult> GetRevenueReportByDay(long idBrand, DateTime fromDate, DateTime toDate)
        {
            var billLineByDate = await _billService.GetRevenueReportByDay(idBrand, fromDate, toDate);
            return new JsonResult(billLineByDate, _jsonSerializerOptions);
        }
    }
}
