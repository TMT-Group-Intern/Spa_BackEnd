using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Application.Authorize.HasPermissionAbtribute;
using Spa.Application.Authorize.Permissions;
using Spa.Domain.Service;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public BranchController(IBranchService branchService, IMapper mapper, IMediator mediator, IWebHostEnvironment env)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _branchService = branchService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
        }

        [HttpGet("allBranches")]
        [HasPermission(SetPermission.GetAllBranches)]
        public async Task<IActionResult> GetAllBranches()
        {
            var allBranches = await _branchService.GetAllBranches();
            return Ok(allBranches);
        }

        [HttpGet("getBranchByID")]
        [HasPermission(SetPermission.GetBranchByID)]
        public async Task<IActionResult> GetBranchByID(long id)
        {
            try
            {
                var getBranchByID = _branchService.GetBranchByID(id);
                if (getBranchByID.Result is null)
                {
                    throw new Exception("Not Found!");
                }
                BranchDTO branchDTO = new BranchDTO
                {
                    BranchID = getBranchByID.Result.BranchID,
                    BranchName = getBranchByID.Result.BranchName,
                    BranchAddress = getBranchByID.Result.BranchAddress,
                    BranchPhone = getBranchByID.Result.BranchPhone,
                    IsActive = getBranchByID.Result.IsActive,
                };
                return Ok(new { branchDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("getBranchNameByID")]
        [HasPermission(SetPermission.GetBranchNameByID)]
        public async Task<IActionResult> GetBranchNameByID(long id)
        {
            try
            {
                string getBranchNameByID = await _branchService.GetBranchNameByID(id);
                if (getBranchNameByID is null)
                {
                    throw new Exception("Not Found!");
                }
                return Ok(new { getBranchNameByID });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("createBranch")]
        [HasPermission(SetPermission.CreateBranch)]
        public async Task<IActionResult> CreateBranch([FromBody] BranchDTO branchDto)
        {
            try
            {
                var branch = new Branch
                {
                    BranchPhone = branchDto.BranchPhone,
                    BranchName = branchDto.BranchName,
                    BranchAddress = branchDto.BranchAddress,
                    IsActive = true,
                };
                var newBranch = await _branchService.CreateBranch(branch);
                return Ok(new { id = newBranch });
            }
            catch (DuplicateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("updateBranch")]
        [HasPermission(SetPermission.UpdateBranch)]
        public async Task<IActionResult> UpdateBranch(long id, [FromBody] BranchDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Branch branch = new Branch
                {
                    BranchID = id,
                    BranchName = updateDto.BranchName,
                    BranchPhone = updateDto.BranchPhone,
                    BranchAddress = updateDto.BranchAddress,
                };

                await _branchService.UpdateBranch(branch);
                return Ok(true);
            }

            catch (DuplicateException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("changeStatusBranch")]
        //[HasPermission(SetPermission.DeleteUser)]
        public async Task<ActionResult> ChangeStatusBranch(long id)
        {
            try
            {
                await _branchService.ChangeStatusBranch(id);
                return Ok(true);
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("deleteBranch")]
        [HasPermission(SetPermission.DeleteBranch)]
        public async Task<ActionResult> DeleteBranch(long id)
        {
            try
            {
                await _branchService.DeleteBranch(id);
                return Ok(true);
            }
            catch (ForeignKeyViolationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("AllBranchPage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllBranchByPages(int pageNumber = 1, int pageSize = 20)
        {
            var branchFromService = await _branchService.GetAllBranchByPages(pageNumber, pageSize);
            var totalItems = await _branchService.GetAllItemBranch();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = branchFromService, totalItems, totalPages });
        }

        [HttpGet("AllBranchActivePage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllBranchActiveByPages(int pageNumber = 1, int pageSize = 20)
        {
            var branchFromService = await _branchService.GetAllBranchActiveByPages(pageNumber, pageSize);
            var totalItems = await _branchService.GetAllItemBranch();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = branchFromService, totalItems, totalPages });
        }

        [HttpGet("AllBranchNotActivePage")]
        //[HasPermission(SetPermission.UserPage)]
        public async Task<ActionResult> GetAllBranchNotActiveByPages(int pageNumber = 1, int pageSize = 20)
        {
            var branchFromService = await _branchService.GetAllBranchNotActiveByPages(pageNumber, pageSize);
            var totalItems = await _branchService.GetAllItemBranch();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return Ok(new { item = branchFromService, totalItems, totalPages });
        }
    }
}
