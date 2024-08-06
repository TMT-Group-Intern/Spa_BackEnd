using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface ITreatmentService
    {
        Task CreateTreatmentCard(TreatmentCard treatmentCard);
        Task<IEnumerable<TreatmentCard>> GetTreatmentCardAsyncByCustomer(long customerId);
        Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID);
    }
}
