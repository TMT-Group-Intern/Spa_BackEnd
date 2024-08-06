using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class TreatmentSessionDTO
    {
        public long SessionID { get; set; }
        public int SessionNumber { get; set; }  // số thứ tự của buổi
        public long TreatmentID { get; set; }
        public bool isDone { get; set; } = false;
        public ICollection<TreatmendSessionDetailDTO> TreatmendSessionDetailDTO { get; set; }
    }
}
