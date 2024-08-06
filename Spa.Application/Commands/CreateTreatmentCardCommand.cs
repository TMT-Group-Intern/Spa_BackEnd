using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateTreatmentCardCommand : IRequest<string>
    {
        public string TreatmentName { get; set; }  //lấy tên dịch vụ + số buổi làm

        public long CustomerID { get; set; }

        public long ServiceID { get; set; }

        //public long? AppointmentID { get; set; }

        public DateTime? StartDate { get; set; }

        public int TotalSessions { get; set; }    // tổng số buổi làm(có thể thay đổi)

        public int Interval { get; set; }

        public string TimeUnit { get; set; }

        public string? Notes { get; set; }

        public string CreateBy { get; set; }

        public ICollection<TreatmentSessionDTO> TreatmentSessionsDTO { get; set; }
    }

    public class CreateTreatmentCardCommandHandler : IRequestHandler<CreateTreatmentCardCommand, string>
    {
        private readonly ITreatmentService _treatmentService;
        public CreateTreatmentCardCommandHandler(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }
        public async Task<string> Handle(CreateTreatmentCardCommand request, CancellationToken cancellationToken)
        {
            ICollection<TreatmentSession> treatmentSession = new List<TreatmentSession>();

            treatmentSession = request.TreatmentSessionsDTO.Select(a => new TreatmentSession
            {
                SessionNumber = a.SessionNumber,
                TreatmentID = a.TreatmentID,
                TreatmendSessionDetail = a.TreatmendSessionDetailDTO.Select(d => new TreatmendSessionDetail
                {
                    SessionID = d.SessionID,
                    ServiceID = d.ServiceID,
                }).ToList()

            }).ToList();



            TreatmentCard treatmentCard = new TreatmentCard
            {
                TreatmentName = request.TreatmentName,
                CustomerID = request.CustomerID,           
                TotalSessions = request.TotalSessions,
                StartDate = request.StartDate,            
                CreateBy = request.CreateBy,
                Notes = request.Notes,
                TreatmentSessions = treatmentSession,
                Status = "Đang điều trị"
            };
            await _treatmentService.CreateTreatmentCard(treatmentCard);
            string status = "success";
            return status;
        }
    }
}
