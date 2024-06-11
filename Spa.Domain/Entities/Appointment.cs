using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Appointment
    {
        public long AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public long? BranchID { get; set; }
        public long CustomerID { get; set; }
        public long EmployeeID { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }



        public Branch Branch { get; set; }
        public Employee Employee { get; set; }
        public Customer Customer { get; set; }

        public ICollection<ChooseService> ChooseServices { get; set; }
    }
}
