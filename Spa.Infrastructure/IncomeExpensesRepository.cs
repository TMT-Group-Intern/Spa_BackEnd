using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class IncomeExpensesRepository
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
    }
}
