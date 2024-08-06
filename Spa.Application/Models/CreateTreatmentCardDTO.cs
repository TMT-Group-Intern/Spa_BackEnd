using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class CreateTreatmentCardDTO
    {
        public string TreatmentName { get; set; }  //lấy tên dịch vụ + số buổi làm

        public long CustomerID { get; set; }

        public DateTime StartDate { get; set; }

        public int TotalSessions { get; set; }    // tổng số buổi làm(có thể thay đổi)

        public string? Notes { get; set; }

        public string CreateBy { get; set; }

        public ICollection<TreatmentSessionDTO> TreatmentSessionsDTO { get; set; }
    }
}
