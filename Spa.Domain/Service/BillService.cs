using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public class BillService : IBillService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IBillRepository _billRepository;

        public BillService(IBillRepository billRepository, IAppointmentRepository appointmentRepository) {
            _appointmentRepository = appointmentRepository;
            _billRepository = billRepository;
        }

        public async Task<bool> AddBillItem(List<BillItem> billItems)
        {
            return await _billRepository.AddBillItem(billItems); ;
        }

        public async Task<Bill> CreateBill(Bill bill)
        {
         
            return await _billRepository.CreateBill(bill);
        }

        public async Task<Bill> GetNewBillAsync()
        {
          return await _billRepository.GetNewBillAsync();
        }

    }
}
