using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITreatmentService _treatmentService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public TreatmentController(IMediator mediator, ITreatmentService treatmentService)
        {
            _mediator = mediator;
            _treatmentService = treatmentService;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        [HttpGet]
        public async Task<ActionResult> GetTreatmentByCustomer(long customerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var listTreatment = await _treatmentService.GetTreatmentCardAsyncByCustomer(customerId);
                return new JsonResult(listTreatment, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{treatmendID}")]
        public async Task<ActionResult> GetTreatmentDetailByID(long treatmendID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var listTreatment = await _treatmentService.GetTreatmentCardDetailAsyncByID(treatmendID);
                return new JsonResult(listTreatment, _jsonSerializerOptions);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<ActionResult> CreateTreatmentCard(CreateTreatmentCardDTO treatmentCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var command = new CreateTreatmentCardCommand
                {
                    CustomerID = treatmentCard.CustomerID,
                    StartDate = treatmentCard.StartDate,
                    TotalSessions = treatmentCard.TotalSessions,
                    TreatmentName = treatmentCard.TreatmentName,
                    CreateBy = treatmentCard.CreateBy,
                    Notes = treatmentCard.Notes,
                    TreatmentSessionsDTO = treatmentCard.TreatmentSessionsDTO
                };
                var id = await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
