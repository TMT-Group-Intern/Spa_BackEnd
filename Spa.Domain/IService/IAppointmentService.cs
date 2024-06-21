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

        Task<bool> AddAssignment(long idApp, long idEm);


        Task<Appointment> GetIdNewAppointment();

        Task<bool> DeleteAppointment(long idApp);

        Task<bool> UpdateAppointmentWithoutService(long id, Appointment appointment);

        Task<bool> UpdateAppointmentWithService(long id,List<long> serviceID, string status);

        Task<bool> UpdateStatus(long id, string status);
    }
}
