﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Payment
    {
        public long PaymentID { get; set; }
        public long CustomerID { get; set; }
        public long AppointmentID { get; set; }      
        public double? Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

   

        public Customer Customer { get; set; }
        public Appointment Appointment { get; set; }
    }
}
