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

        public PaymentService(IPaymentRepository paymentRepository)
        {

            _paymentRepository = paymentRepository;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            return await _paymentRepository.AddPayment(payment);
        }

        public async Task<double?> GetRevenueToday()
        {
            return await _paymentRepository.GetRevenue();
        }

        public async Task<List<Payment>> GetAllPaymentsByBranch(long branchID)
        {
        var list =   await _paymentRepository.GetAllPaymentByBranch(branchID);
            if (list == null)
            {
                throw new ErrorMessage("There is no data in the system");
            }
            return list;
        }


    }
}
