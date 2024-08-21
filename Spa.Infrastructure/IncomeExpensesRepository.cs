using Azure;
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
    public class IncomeExpensesRepository : IIncomeExpensesRepository
    {
        private readonly SpaDbContext _spaDbContext;
        public IncomeExpensesRepository(SpaDbContext spaDbContext)
        {
            _spaDbContext = spaDbContext;
        }

        public async Task<bool> AddncomeExpensesAsync(IncomeExpenses incomeExpenses)
        {
            try
            {
                await _spaDbContext.IncomeExpenses.AddAsync(incomeExpenses);
                await _spaDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> GetLastCodeAsync()
        {
            return await _spaDbContext.IncomeExpenses.OrderByDescending(e => e.IncomeExpensID!).Select(e => e.IncomeExpensesCode!).FirstOrDefaultAsync();
        }

        public async Task<object> GetIncomes(int offset, int limit)
        {
            IQueryable<IncomeExpenses> query = _spaDbContext.IncomeExpenses.OrderByDescending(e => e.IncomeExpensID).Skip((offset - 1) * limit)
              .Take(limit); 
            var total = await CountTotalIncomes();
            var items = await query.ToListAsync();
            return new { items, total };
        }

        private async Task<int> CountTotalIncomes()
        {
            return await _spaDbContext.IncomeExpenses.CountAsync();
        }

        public async Task<object> TotalAmountThuChi()
        {
            IQueryable<IncomeExpenses> queryTotalBank = _spaDbContext.IncomeExpenses.Where(e => e.PayMethod.Equals("Chuyển khoản"));
            IQueryable<IncomeExpenses> queryTotalCash = _spaDbContext.IncomeExpenses.Where(e => e.PayMethod.Equals("Tiền mặt"));
            var totalCash = await queryTotalCash.Select(e => e.Amount).SumAsync();
            var totalBank = await queryTotalBank.Select(e => e.Amount).SumAsync();
            return new { totalCash, totalBank };
        }


    }
}
