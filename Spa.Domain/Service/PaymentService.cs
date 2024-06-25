using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public PaymentService(IPaymentRepository paymentRepository, IAppointmentRepository appointmentRepository)
        {

            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            var app = _appointmentRepository.GetAppointmentByID(payment.AppointmentID);
            if (app.Status == "Already paid")
            {
                throw new ErrorMessage("The customer has paid");
            }
            return await _paymentRepository.AddPayment(payment);
        }

        public async Task<double?> GetRevenueToday()
        {
            return await _paymentRepository.GetRevenue();
        }

        public async Task<List<Payment>> GetAllPaymentsByBranch(long branchID)
        {
            var list = await _paymentRepository.GetAllPaymentByBranch(branchID);
            if (list == null)
            {
                throw new ErrorMessage("There is no data in the system");
            }
            return list;
        }


    }
}
