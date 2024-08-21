using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeExpensesController : ControllerBase
    {
        private readonly IIncomeExpensesService _incomeExpensesService;

        public IncomeExpensesController(IIncomeExpensesService incomeExpensesService)
        {
            _incomeExpensesService = incomeExpensesService;
        }

        [HttpPost]
        public async Task<ActionResult> TaoPhieuThuChi(IncomeExpenses incomeExpenses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if(incomeExpenses.TypeOfIncome == "Chi")
                {
                    incomeExpenses.Amount = - incomeExpenses.Amount;
                }
                var create = new IncomeExpenses
                {
                    Amount = incomeExpenses.Amount,
                    PartnerName = "__",
                    PayMethod = incomeExpenses.PayMethod,
                    TypeOfIncome = incomeExpenses.TypeOfIncome,
                    Date = DateTime.Now,
                    BranchID = incomeExpenses.BranchID,   
                    Description = incomeExpenses.Description,
                };
                await _incomeExpensesService.AddncomeExpensesAsync(create);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("AllCashAndBank")]
        public async Task<ActionResult> GetAllBankAndCash()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                
               var respone = await _incomeExpensesService.TotalAmountThuChi();

                return Ok(new { respone });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
