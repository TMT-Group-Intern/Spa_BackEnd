using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class UpdateAppointmentWithoutServiceDTO
    {

        public DateTime AppointmentDate { get; set; }
        public long BranchID { get; set; }
        public long CustomerID { get; set; }
        public string Status { get; set; }
        public double? Total { get; set; }

        public ICollection<AssignmentDTO>? Assignments { get; set; }
    }
}
