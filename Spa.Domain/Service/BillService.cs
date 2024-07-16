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

        public BillService(IBillRepository billRepository, IAppointmentRepository appointmentRepository)
        {
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

        public async Task<Bill?> GetBillByIdAsync(long id)
        {
            return await _billRepository.GetBillByIdAsync(id);
        }

        public async Task<Bill> GetNewBillAsync()
        {
            return await _billRepository.GetNewBillAsync();
        }

        public async Task<IEnumerable<Bill>> GetAllBillAsync()
        {
            return await _billRepository.GetAllBillAsync();
        }

        public async Task<Bill> UpdateBill(long id, Bill bill)
        {
            var billToUpdate = await _billRepository.GetBillByIdAsync(id);

            if (billToUpdate != null)
            {
                billToUpdate.TotalAmount = bill.TotalAmount;
                if (billToUpdate.BillItems != null)
                {
                    foreach (var item in billToUpdate.BillItems)
                    {
                        item.Quantity = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.Quantity).FirstOrDefault();
                        item.UnitPrice = bill.BillItems!.Where(ser => ser.ServiceID == item.ServiceID).Select(i => i.UnitPrice).FirstOrDefault();
                    }
                }
                await _billRepository.UpdateBill(billToUpdate);
            }

            return billToUpdate!;
        }
    }
}
