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

        public Appointment CreateAppointment(Appointment appointment)
        {
            Add(appointment);
            return appointment;
        }

        public Task<bool> DeleteAppointment(int id)
        {
            throw new NotImplementedException();
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
                                             .Include(e => e.Employee).FirstOrDefault();

        }

        public void UpdateAppointment(Appointment customer)
        {
            throw new NotImplementedException();
        }
    }
}
