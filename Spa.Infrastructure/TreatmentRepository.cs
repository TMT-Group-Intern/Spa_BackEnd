﻿using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly SpaDbContext _spaDbContext;

        public TreatmentRepository(SpaDbContext spaDbContext)
        {
            _spaDbContext = spaDbContext;
        }

        public async Task CreateTreatmentCard(TreatmentCard treatmentCard)
        {
            try
            {
                await _spaDbContext.TreatmentCards.AddRangeAsync(treatmentCard);
                await _spaDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<IEnumerable<TreatmentCard>> GetTreatmentCardAsyncByCustomer(long customerId)
        {
            var respone = await _spaDbContext.TreatmentCards.Where(a => a.CustomerID == customerId)
                .ToListAsync();
            return respone;
        }

        public async Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID)
        {
            var response = await _spaDbContext.TreatmentCards
                .Where(a => a.TreatmentID == treatmendID)
                .Include(a => a.TreatmentSessions)
                .ThenInclude(a => a.TreatmendSessionDetail).ThenInclude(e=> e.Service).FirstOrDefaultAsync();
            return response;
        }

        public bool UpdateTreatment(TreatmentCard treatmentCard)
        {
            try
            {
                _spaDbContext.TreatmentCards.UpdateRange(treatmentCard);
                _spaDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return true;
        }

    }
}
