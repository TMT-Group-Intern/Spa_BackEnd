using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class TreatmentCard
    {
        public long? TreatmentID { get; set; }
        public string TreatmentName { get; set; }  //lấy tên dịch vụ + số buổi làm

        public long CustomerID { get; set; }    
        public DateTime? StartDate { get; set; }
        public int TotalSessions { get; set; }    // tổng số buổi làm(có thể thay đổi)
        public string? Notes { get; set; }
        public string CreateBy { get; set; }

        public string? Status { get; set; }
        //chhỉ chọn các dịch vụ là liệu trình 
        public Customer? Customer { get; set; }
        public ICollection<TreatmentSession> TreatmentSessions { get; set; }


    }
}
