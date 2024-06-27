using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class UserForEmployeeDTO
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
