using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spa.Domain.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using Spa.Domain.Service;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Application.Commands;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchAndJobController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IBranchAndJobService _bnjService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public BranchAndJobController(IBranchAndJobService bnjService, IMapper mapper, IMediator mediator, IWebHostEnvironment env)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            _bnjService = bnjService;
            _mapper = mapper;
            _mediator = mediator;
            _env = env;
        }

        [HttpGet("allBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            var allBranches = await _bnjService.GetAllBranches();
            return Ok(allBranches);
        }

        [HttpGet("getBranchByID")]
        public async Task<IActionResult> GetBranchByID(long id)
        {
            try
            {
                var getBranchByID = _bnjService.GetBranchByID(id);
                if(getBranchByID.Result is null)
                {
                    throw new Exception("Not Found!");
                }
                BranchDTO branchDTO = new BranchDTO
                {
                    BranchID = getBranchByID.Result.BranchID,
                    BranchName = getBranchByID.Result.BranchName,
                    BranchAddress = getBranchByID.Result.BranchAddress,
                    BranchPhone = getBranchByID.Result.BranchPhone,
                };
                return Ok(new { branchDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("createBranch")]
        public async Task<IActionResult> CreateBranch([FromBody] BranchDTO branchDto)
        {
            try
            {
                var branch = new Branch
                {
                    BranchPhone = branchDto.BranchPhone,
                    BranchName= branchDto.BranchName,
                    BranchAddress = branchDto.BranchAddress,
                };
                var newBranch = await _bnjService.CreateBranch(branch);
                //  return Ok(true);
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

                    await _bnjService.UpdateBranch(branch);
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

        [HttpDelete("deleteBranch")]
        public async Task<ActionResult> DeleteBranch(long id)
        {
            try
            {
                await _bnjService.DeleteBranch(id);
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

        [HttpGet("allJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            var allJobs = await _bnjService.GetAllJobs();
            return Ok(allJobs);
        }

        [HttpGet("getJobTypeNameByID")]
        public async Task<IActionResult> GetJobTypeByID(long id)
        {
            try
            {
                var getJobTypeNameByID = _bnjService.GetJobTypeByID(id);
                if (getJobTypeNameByID.Result is null)
                {
                    throw new Exception("Not Found!");
                }
                JobDTO jobDTO = new JobDTO
                {
                    JobTypeID = getJobTypeNameByID.Result.JobTypeID,
                    JobTypeName = getJobTypeNameByID.Result.JobTypeName,
                };
                return Ok(new { jobDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("createJob")]
        public async Task<IActionResult> CreateJobType([FromBody] JobDTO jobDto)
        {
            try
            {
                var job = new JobType
                {
                    //JobTypeID= jobDto.JobTypeID,
                    JobTypeName = jobDto.JobTypeName,
                };
                var newjob =await  _bnjService.CreateJobType(job);
                //  return Ok(true);
                return Ok(new { id = newjob });
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

        [HttpPut("updateJob")]
        public async Task<IActionResult> UpdateJob(long id, [FromBody] JobDTO updateDto)
        {
            try
            {
                if (updateDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                JobType job = new JobType
                {
                    JobTypeID = id,
                    JobTypeName = updateDto.JobTypeName,
                };
                await _bnjService.UpdateJob(job);
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

        [HttpDelete("deleteJob")]
        public async Task<ActionResult> DeleteJob(long id)
        {
            try
            {
                await _bnjService.DeleteJob(id);
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
    }
}
