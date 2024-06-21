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
    public  class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService( IPaymentRepository paymentRepository) 
        {

            _paymentRepository = paymentRepository;
        }

        public async Task<bool>  AddPayment(Payment payment)
        {
            return await _paymentRepository.AddPayment(payment);
        }
    }
}
