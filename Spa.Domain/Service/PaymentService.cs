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
        private readonly IBillRepository _billRepository;

        public PaymentService(IPaymentRepository paymentRepository, IAppointmentRepository appointmentRepository, IBillRepository billRepository)
        {

            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _billRepository = billRepository;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            try
            {
                payment.PaymentDate = DateTime.Now;
                payment.CreatedAt = DateTime.Now;
                var paymentProcess = await _paymentRepository.AddPayment(payment);
                if (paymentProcess)
                {
                    var bill = await _billRepository.GetBillByIdAsync(payment.BillID);
                    bill.AmountInvoiced += payment.Amount;
                    bill.AmountResidual -= payment.Amount;
                    if(bill.AmountResidual == 0)
                    {
                        bill.BillStatus = "Thanh toán hoàn tất";
                    }
                    await _billRepository.UpdateBill(bill);
                }
               
                return true;
            } catch (Exception ex) {
                return false;
            }
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

        public async Task<Object> GetPaymentByBill(long billID)
        {
            return await _paymentRepository.GetPaymentByBill(billID);
        }
    }
}
