using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ITreatmentRepository _treatmentRepository;

        public TreatmentService(ITreatmentRepository treatmentRepository)
        {
            _treatmentRepository = treatmentRepository;
        }
        public async Task CreateTreatmentCard(TreatmentCard treatmentCard)
        {
           await _treatmentRepository.CreateTreatmentCard(treatmentCard);
        }

        public async Task<IEnumerable<TreatmentCard>> GetTreatmentCardAsyncByCustomer(long customerId)
        {
            return  await _treatmentRepository.GetTreatmentCardAsyncByCustomer(customerId);
        }

        public async Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID)
        {
            return await _treatmentRepository.GetTreatmentCardDetailAsyncByID(treatmendID);
        }
    }
}
