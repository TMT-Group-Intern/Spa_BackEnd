using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class TreatmentSession
    {
        public long? SessionID { get; set; }
        public int SessionNumber { get; set; }  // số thứ tự của buổi
        public long TreatmentID { get; set; }
        public bool isDone { get; set; } = false;
        public TreatmentCard? TreatmentCard { get; set; }
        public ICollection<TreatmendSessionDetail> TreatmendSessionDetail { get; set; }
    }
}
