using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IAppointmentService
    {
       Task CreateAppointmentAsync(Appointment appointment);

       IEnumerable<Appointment> GetAllAppoinment();

        Appointment GetAppointmentByIdAsync(long id);

        Task<bool> AddChooseServiceToappointment(long idApp, long idSer);

        Task<Appointment> GetIdNewAppointment();

        Task<bool> DeleteAppointment(long idApp);
    }
}
