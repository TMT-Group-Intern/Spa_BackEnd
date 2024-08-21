using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class IncomeExpensesService : IIncomeExpensesService
    {
        private readonly IIncomeExpensesRepository _incomeExpensesRepository;

        public IncomeExpensesService(IIncomeExpensesRepository incomeExpensesRepository )
        {
            _incomeExpensesRepository = incomeExpensesRepository;
        }

        public async Task<bool> AddncomeExpensesAsync(IncomeExpenses incomeExpenses)
        {
            if(incomeExpenses.TypeOfIncome == "Thu")
            {
                incomeExpenses.IncomeExpensesCode = await  GeneratePhieuThuCodeAsync();
            }
            else
            {
                incomeExpenses.IncomeExpensesCode = await GeneratePhieuChiCodeAsync();
            }
            return await _incomeExpensesRepository.AddncomeExpensesAsync(incomeExpenses);
        }

        public async Task<object> GetIncomes(int offset, int limit)
        {
            return await _incomeExpensesRepository.GetIncomes(offset, limit);
        }

        private bool IsValidFormat(string input)
        {
            string pattern = @"^[A-Z]{2}\d{4}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        private async Task<string> GeneratePhieuThuCodeAsync()
        {
            var lastBillCode = await _incomeExpensesRepository.GetLastCodeAsync();
            if (lastBillCode == null || IsValidFormat(lastBillCode) == false)
            {
                return "PT0001";
            }
            var lastCode = lastBillCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "PT" + numericPart.ToString("D4");
        }

        private async Task<string> GeneratePhieuChiCodeAsync()
        {
            var lastBillCode = await _incomeExpensesRepository.GetLastCodeAsync();
            if (lastBillCode == null || IsValidFormat(lastBillCode) == false)
            {
                return "PC0001";
            }
            var lastCode = lastBillCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "PC" + numericPart.ToString("D4");
        }

        public async Task<object> TotalAmountThuChi()
        {
            return await _incomeExpensesRepository.TotalAmountThuChi();
        }
    }
}
