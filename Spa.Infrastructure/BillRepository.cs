using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class BillRepository : EfRepository<Bill>, IBillRepository
    {
        public BillRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public async Task<IEnumerable<Bill>> GetAllBillAsync()
        {
            return await _spaDbContext.Bill.ToListAsync();
        }

        public async Task<bool> AddBillItem(List<BillItem> billItems)
        {
            try
            {
                foreach (var item in billItems)
                {
                    await _spaDbContext.BillItem.AddAsync(item);
                }
               await _spaDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Bill> CreateBill(Bill bill)
        {
            Add(bill);
            return bill;
        }

        public async Task<Bill?> GetBillByIdAsync(long id) //Get By ID
        {
         return await _spaDbContext.Bill.Include(i => i.BillItems).Where(b => b.BillID == id).FirstOrDefaultAsync() ?? null;
        }

        public async Task<Bill> GetNewBillAsync()
        {
            try
            {
                var bill = await _spaDbContext.Bill.OrderByDescending(c => c.BillID).FirstOrDefaultAsync();
                if (bill is not null)
                {
                    return bill;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Bill> UpdateBill(Bill bill)
        {
           _spaDbContext.Bill.UpdateRange(bill);
            await _spaDbContext.SaveChangesAsync();
            return bill;
        }

    }
}
