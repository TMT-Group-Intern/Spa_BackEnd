using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        /*public async Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID)
        {
            *//*            var response = await _spaDbContext.TreatmentCards
                            .Where(a => a.TreatmentID == treatmendID)
                            .Include(a => a.TreatmentSessions)
                            .ThenInclude(a => a.TreatmendSessionDetail).ThenInclude(e => e.Service).FirstOrDefaultAsync();
                        return response;*//*
        }*/

        public async Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID)
        {
            var response = await _spaDbContext.TreatmentCards
                .Where(a => a.TreatmentID == treatmendID)
                .Select(a => new
                {
                    a.TreatmentID,
                    a.TreatmentName,
                    a.CustomerID,
                    a.StartDate,
                    a.TotalSessions,
                    a.CreateBy,
                    a.Notes,
                    a.Status,
                    TreatmentSessions = a.TreatmentSessions.Select(s => new
                    {
                        s.SessionID,
                        s.SessionNumber,
                        s.TreatmentID,
                        s.isDone,
                        TreatmendSessionDetail = s.TreatmendSessionDetail.Select(t => new
                        {
                            t.TreatmendDetailID,
                            t.ServiceID,
                            t.SessionID,
                            t.IsDone,
                            t.Price,
                            Service = t.Service
                        }),
                    })
                }).FirstOrDefaultAsync();

            if (response != null)
            {
                var treatmentCard = new TreatmentCard
                {
                    TreatmentID = response.TreatmentID,
                    TreatmentName = response.TreatmentName,
                    CustomerID = response.CustomerID,
                    StartDate = response.StartDate,
                    TotalSessions = response.TotalSessions,
                    CreateBy = response.CreateBy,
                    Notes = response.Notes,
                    Status = response.Status,
                    TreatmentSessions = response.TreatmentSessions.Select(s => new TreatmentSession
                    {
                        SessionID = s.SessionID,
                        SessionNumber = s.SessionNumber,
                        TreatmentID = s.TreatmentID,
                        isDone = s.isDone,
                        TreatmendSessionDetail = s.TreatmendSessionDetail.Select(t => new TreatmendSessionDetail
                        {
                            TreatmendDetailID = t.TreatmendDetailID,
                            ServiceID = t.ServiceID,
                            SessionID = t.SessionID,
                            IsDone = t.IsDone,
                            Price = t.Price,
                            Service = t.Service
                        }).ToList()
                    }).ToList()
                };

                return treatmentCard;
            }
            else
            {
                return null;
            }
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

        public bool UpdateStatusSession(long id, bool status)
        {
            try
            {
                var session = GetSessionByID(id);
                session.isDone = status;
                _spaDbContext.TreatmentSessions.Update(session);
                _spaDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return true;
        }

        private TreatmentSession GetSessionByID(long id)
        {
            return _spaDbContext.TreatmentSessions.Where(e => e.SessionID == id).FirstOrDefault();
        }

    }
}
