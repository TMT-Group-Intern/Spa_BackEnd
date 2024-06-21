using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        public async Task<double?> GetRevenue()
        {
            DateTime date = DateTime.Now;
            var totalAmount = await _spaDbContext.Payments
           .Where(p => p.PaymentDate.Date == date.Date)
           .SumAsync(p => p.Amount);

            return totalAmount;
        }

        public async Task<List<Payment>> GetAllPaymentByBranch(long branchID)
        {
            return await _spaDbContext.Payments.Include(c =>c.Customer)
                .Include(a => a.Appointment).Where(a => a.Appointment.BranchID == branchID).ToListAsync();
        }

    }
}
