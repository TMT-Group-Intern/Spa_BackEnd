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
            return await _treatmentRepository.GetTreatmentCardAsyncByCustomer(customerId);
        }

        public async Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID)
        {
            return await _treatmentRepository.GetTreatmentCardDetailAsyncByID(treatmendID);
        }

        public async Task<bool> UpdateTreatment(long treatmendID, TreatmentCard treatmentCard)
        {
            var treatmentToUpdate = await _treatmentRepository.GetTreatmentCardDetailAsyncByID(treatmendID);
            if(treatmentToUpdate != null)
            {
                treatmentToUpdate.TreatmentName = treatmentCard.TreatmentName;
                treatmentToUpdate.StartDate = treatmentCard.StartDate;
                treatmentToUpdate.Status = treatmentCard.Status;
                treatmentToUpdate.Notes = treatmentCard.Notes;          
                treatmentToUpdate.TotalSessions = treatmentCard.TotalSessions;
                treatmentToUpdate.TreatmentSessions.Clear();
                treatmentToUpdate.TreatmentSessions = treatmentCard.TreatmentSessions.Select(a => new TreatmentSession
                {
                    SessionNumber = a.SessionNumber,
                    TreatmendSessionDetail = a.TreatmendSessionDetail.Select(a => new TreatmendSessionDetail
                    {               
                        ServiceID = a.ServiceID,
                    }).ToList()

                }).ToList();

            }
            //   treatmentToUpdate = treatmentCard;
            var update = _treatmentRepository.UpdateTreatment(treatmentToUpdate);
            return update;
        }

        private void UpdateNonNullFields(TreatmentCard target, TreatmentCard source)
        {
            var properties = typeof(TreatmentCard).GetProperties();
            foreach (var property in properties)
            {
                var sourceValue = property.GetValue(source);
                if (sourceValue != null)
                {
                    property.SetValue(target, sourceValue);
                }
            }
        }


       public bool UpdateStatusSession(long id, bool status)
        {
            _treatmentRepository.UpdateStatusSession(id, status);
            return true;
        }

    }
}
