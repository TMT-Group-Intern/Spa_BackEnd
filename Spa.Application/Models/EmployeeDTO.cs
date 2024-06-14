using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class EmployeeDTO
    {
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime HireDate { get; set; }
        public long JobTypeID { get; set; }
    }
}
