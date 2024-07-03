﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Commands;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IService;
using Spa.Domain.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _env;

        public CustomersController(ICustomerService service, IMediator mediator, IWebHostEnvironment env)
        {
            _service = service;
            _mediator = mediator;
            _env = env;
          
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var customersFromService = _service.GetAllCustomer();

            var customerDTO = customersFromService.Select(c => new CustomerDTO
            {
                CustomerID = c.CustomerID,
                CustomerCode = c.CustomerCode,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DateOfBirth = c.DateOfBirth,
                Email = c.Email,
                Gender = c.Gender,
                Phone = c.Phone
            });
            return Ok(new { item = customerDTO });
        }

        [HttpGet("Page")]
        public async Task<ActionResult> GetAllByPage(int pageNumber = 1, int pageSize = 20)
        {
            var customersFromService = await _service.GetByPages(pageNumber, pageSize);

            var customerDTO = customersFromService.Select(c => new CustomerDTO
            {
                CustomerID = c.CustomerID,
                CustomerCode = c.CustomerCode,
                FirstName = c.FirstName,
                LastName = c.LastName,
                DateOfBirth = c.DateOfBirth,
                Email = c.Email,
                Gender = c.Gender,
                Phone = c.Phone
            });

            var totalItems = await _service.GetAllItem();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);


            return Ok(new { item = customerDTO, totalItems, totalPages });
        }

        [HttpGet("{id}")]
        public ActionResult GetCusomerById(long id)
        {
            if (_service.isExistCustomer(id))
            {
                var getByCusByID = _service.GetCustomerById(id);

                CustomerDTO customerDTO = new CustomerDTO
                {
                    CustomerID = getByCusByID.CustomerID,
                    CustomerCode = getByCusByID.CustomerCode,
                    DateOfBirth = getByCusByID.DateOfBirth,
                    Email = getByCusByID.Email,
                    FirstName = getByCusByID.FirstName,
                    LastName = getByCusByID.LastName,
                    Gender = getByCusByID.Gender,
                    Phone = getByCusByID.Phone

                };
                return Ok(new { customerDTO });
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDto)
        {
            try
            {
                var command = new CreateCustomerCommand
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    DateOfBirth = customerDto.DateOfBirth,
                    Gender = customerDto.Gender,
                    Phone = customerDto.Phone
                };
                var id = await _mediator.Send(command);
                //  return Ok(true);
                return Ok(new { id = id });
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


        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(long customerId, [FromBody] CustomerDTO customerDto)
        {
            try
            {
                if (customerDto == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_service.isExistCustomer(customerId))
                {
                    return NotFound();
                }
                Customer customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    DateOfBirth = customerDto.DateOfBirth,
                    Gender = customerDto.Gender,
                    Phone = customerDto.Phone
                };
                await _service.UpdateCustomer(customerId, customer);
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

        [HttpDelete("{customerId}")]
        public async Task<ActionResult> DeactivateCustomer(long customerId)
        {
            try
            {
                if (_service.isExistCustomer(customerId))
                {
                    await _service.DeleteCustomer(customerId);
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
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


        [HttpGet("search")]
        public async Task<ActionResult<List<Customer>>> SearchCustomers(string searchTerm)
        {
            try
            {
                var customers = await _service.SearchCustomersAsync(searchTerm);
                return Ok(new { customers });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult> UploadImage(IFormFile file, long id)
        {
            try
            {
                //var httpRequest = Request.Form;
                //var postFile = httpRequest.Files[0];
                string fileName = file.FileName;
                var physicalPath = Path.Combine(_env.ContentRootPath, "Photos", fileName);

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                await _service.UploadImage(id, fileName);
                return Ok(new { fileName });
            }
            catch (Exception ex)
            {
                return new JsonResult("Update not succeess");
            }
        }

        [HttpPost("uploadMutil")]
        public async Task<ActionResult> UploadImages(List<IFormFile> files, long id)
        {
            try
            {
                var uploadedFiles = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        string fileName = file.FileName;
                        var physicalPath = Path.Combine(_env.ContentRootPath, "Photos", fileName);

                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Gọi dịch vụ để xử lý thông tin tệp (nếu có)
                        await _service.UploadImage(id, fileName);

                        uploadedFiles.Add(fileName);
                    }
                }

                return Ok(new { files = uploadedFiles });
            }
            catch (Exception ex)
            {
                return new JsonResult("Update not successful") { StatusCode = 500 };
            }
        }


        [HttpGet("/GetHistory")]
        public async Task<ActionResult> GetHistoryCustomerById(long cutomerId)
        {
            var listHistoryByAppointment = await _service.GetHistoryCustomerById(cutomerId);
            List<HistoryForCustomerByIdDTO> listHistoryForCus = new List<HistoryForCustomerByIdDTO>();


            foreach (var i in listHistoryByAppointment)
            {
                HistoryForCustomerByIdDTO historyById = new HistoryForCustomerByIdDTO
                {
                    // CustomerName = i.Customer.FirstName + " " + i.Customer.LastName,
                    ServiceUsed = i.ChooseServices.Select(e => e.Service.ServiceName).ToList(),
                    Date = i.AppointmentDate,
                    PhotoCustomer = i.CustomerPhotos.Select(p => p.PhotoPath).ToList()
                };

                listHistoryForCus.Add(historyById);
            }

            return Ok(new { listHistoryForCus });
        }

    }
}
