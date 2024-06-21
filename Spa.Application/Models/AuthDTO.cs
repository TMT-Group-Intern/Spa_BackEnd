using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public record UserSession(string? Id, string? Name, string? Email, string? Role);
    public record LoginRequest(string? Email,string? Password);
    public class LoginDTO
        {
            [Required]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]

            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]

            public string Password { get; set; } = string.Empty;
        }

        public class UpdateDTO:UserDTO
        {
            public DateTime DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string Phone {  get; set; }
            public string AdminCode { get; set; }
            public string EmployeeCode { get; set; }
            public DateTime HireDate { get; set; }
            public long JobTypeID { get; set; }
            public long BranchID { get; set; }

        }
    
}
