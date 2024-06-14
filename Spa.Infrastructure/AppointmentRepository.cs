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
    public class AppointmentRepository : EfRepository<Appointment>, IAppointmentRepository
    {

        public AppointmentRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public async Task<bool> AddChooseService(long idApp, long idSer)
        {
            ChooseService chooseService = new ChooseService
            {
                AppointmentID = idApp,
                ServiceID = idSer
            };
            await _spaDbContext.AddAsync(chooseService);
            await _spaDbContext.SaveChangesAsync();
            return true;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            Add(appointment);
            return appointment;
        }

        public bool DeleteAppointment(Appointment appointment)
        {
           var idApp = appointment.AppointmentID;
            var listIdDelete = _spaDbContext.ChooseServices.Where(c => c.AppointmentID == appointment.AppointmentID).ToList();
            _spaDbContext.RemoveRange(listIdDelete);
            _spaDbContext.Appointments.Remove(appointment);
            _spaDbContext.SaveChanges();
            return true;
        }

        public IEnumerable<Appointment> GetAllAppointment()
        {
            return _spaDbContext.Appointments
                                .Include(c => c.Customer)
                                .Include(e => e.Employee).ToList();
        }

        public Appointment GetAppointmentByID(long appointmentId)
        {
            return _spaDbContext.Appointments.Where(a => a.AppointmentID == appointmentId)
                                             .Include(c => c.Customer)
                                             .Include(e => e.Employee)
                                             .Include(s => s.ChooseServices).ThenInclude(se => se.Service)
                                             .FirstOrDefault();
        }

        public void UpdateAppointment(Appointment customer)
        {
            throw new NotImplementedException();
        }

        public async Task<Appointment> GetNewAppoinmentAsync()
        {
            try
            {
                return await _spaDbContext.Appointments.OrderByDescending(c => c.AppointmentID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
