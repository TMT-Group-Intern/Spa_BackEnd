using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly SpaDbContext _spaDbContext;

        public PaymentRepository(SpaDbContext spaDbContext)
        {
            _spaDbContext = spaDbContext;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
           await _spaDbContext.Payments.AddAsync(payment);
             _spaDbContext.SaveChanges();
            return true;
        }

    }
}
