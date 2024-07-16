using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IBillRepository
    {
        Task<Bill> CreateBill(Bill bill);

        Task<Bill> GetNewBillAsync();

        Task<bool> AddBillItem(List<BillItem> billItems);

        Task<Bill?> GetBillByIdAsync(long id);

        Task<IEnumerable<Bill>> GetAllBillAsync();

        Task<Bill> UpdateBill(Bill bill);


    }
}
