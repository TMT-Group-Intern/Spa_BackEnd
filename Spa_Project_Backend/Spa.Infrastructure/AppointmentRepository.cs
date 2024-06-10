using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class AppointmentRepository: EfRepository<Appointment>, IAppointmentRepository
    {

        public AppointmentRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public Appointment CreateAppointment(Appointment customer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAppointment(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Appointment> GetAllAppointment()
        {
            return _spaDbContext.Appointments.ToList();
        }

        public void UpdateAppointment(Appointment customer)
        {
            throw new NotImplementedException();
        }
    }
}
