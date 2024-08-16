using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class BillDTO
    {
        public long? BillID { get; set; }
        public long? CustomerID { get; set; }
        public long? AppointmentID { get; set; }
        public DateTime Date { get; set; }
        public string? BillStatus { get; set; }
        public string? Doctor { get; set; }
        public string? TechnicalStaff { get; set; }

        public double? TotalAmount { get; set; }   // tổng tiền
        public double? AmountInvoiced { get; set; } = 0;// thanh toán
        public double? AmountResidual { get; set; } = 0; // còn lại

        public double? AmountDiscount { get; set; } = 0;
        public string? KindofDiscount { get; set; }

        public string? Note { get; set; }
    }
}
