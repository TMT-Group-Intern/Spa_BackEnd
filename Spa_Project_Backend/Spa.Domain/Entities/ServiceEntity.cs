using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class ServiceEntity
    {
        public long ServiceID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public ICollection<ChooseService> ChooseServices { get; set; }
    }
}
