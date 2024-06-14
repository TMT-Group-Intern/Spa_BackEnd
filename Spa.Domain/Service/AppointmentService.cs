using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task CreateAppointmentAsync(Appointment appointment)
        {
            _appointmentRepository.CreateAppointment(appointment);
                 
        }

        public IEnumerable<Appointment> GetAllAppoinment()
        {
            return _appointmentRepository.GetAllAppointment();
        }

        public  Appointment GetAppointmentByIdAsync(long id)
        {                 
            return _appointmentRepository.GetAppointmentByID(id) ;
        }

        public async Task<Appointment> GetIdNewAppointment()
        {
            return await _appointmentRepository.GetNewAppoinmentAsync();
           
        }

        public Task<bool> AddChooseServiceToappointment(long idApp, long idSer)
        {
            return _appointmentRepository.AddChooseService(idApp, idSer) ;
        }

        public async Task<bool> DeleteAppointment(long idApp)
        {
         var checkAppointment =  _appointmentRepository.GetAppointmentByID(idApp);
          if (checkAppointment != null)
            {
                if (checkAppointment.Status.Equals("Completed"))
                {
                    throw new ErrorMessage("Can not delete when appointment is completed");
                }
                _appointmentRepository.DeleteAppointment(checkAppointment);
                return true;
            }
          return false;
        }
    }
}
