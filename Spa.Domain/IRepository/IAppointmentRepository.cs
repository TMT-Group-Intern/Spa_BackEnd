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
        void UpdateAppointment(Appointment appointment);
     //   Task<bool> DeleteAppointment(int id);
        Appointment CreateAppointment(Appointment customer);
        IEnumerable<Appointment> GetAllAppointment();

        Appointment GetAppointmentByID(long appointmentId) ;

        Task<bool> AddChooseService(long idApp, long idSer);

        Task<Appointment?> GetNewAppoinmentAsync();

        bool DeleteAppointment(Appointment appointment);

        Task<bool> AddAssignment(long idApp, long idEm);

        Task<bool> UpdateAppointmentWithoutService(Appointment appointment);

        Task<bool> updateServiceInAppointmentByDoctor(long id, List<long> serviceID);

        Task<IEnumerable<ChooseService>> ListService(long id);

        Task RemoveChooseService(long id, List<long> serviceID);

        Task<List<double>> GetAllPriceService(long idApp);
        bool UpdateTotalAppointment(Appointment appointment);

        Task<bool> UpdateAppointmentAsync(Appointment appointment); // Update Apppointment

        Task<Appointment> GetAppointmentByIdAsync(long idApp);
    }
}
