using Spa.Domain.Entities;
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
    }
}
