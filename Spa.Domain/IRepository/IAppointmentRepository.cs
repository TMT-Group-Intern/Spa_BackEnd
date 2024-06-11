using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IAppointmentRepository
    {
        void UpdateAppointment(Appointment customer);
        Task<bool> DeleteAppointment(int id);
        Appointment CreateAppointment(Appointment customer);
         Task<IEnumerable<Appointment>> GetAllAppointment();
    }
}
