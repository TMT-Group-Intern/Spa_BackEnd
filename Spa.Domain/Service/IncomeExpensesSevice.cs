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
    public class IncomeExpensesSevice : IIncomeExpensesSevice
    {
        private readonly IIncomeExpensesRepository _incomeExpensesRepository;

        public IncomeExpensesSevice(IIncomeExpensesRepository incomeExpensesRepository )
        {
            _incomeExpensesRepository = incomeExpensesRepository;
        }

        public Task<bool> AddncomeExpensesAsync(IncomeExpenses incomeExpenses)
        {
            throw new NotImplementedException();
        }
    }
}
