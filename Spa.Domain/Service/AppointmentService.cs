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

        public Appointment GetAppointmentByIdAsync(long id)
        {
            return _appointmentRepository.GetAppointmentByID(id);
        }

        public async Task<Appointment> GetIdNewAppointment()
        {
            return await _appointmentRepository.GetNewAppoinmentAsync();

        }

        public async Task<bool> AddChooseServiceToappointment(long idApp, long idSer)
        {
            return await _appointmentRepository.AddChooseService(idApp, idSer);
        }

        public async Task<bool> DeleteAppointment(long idApp)
        {
            var checkAppointment = _appointmentRepository.GetAppointmentByID(idApp);
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



        public Task<bool> AddAssignment(long idApp, long idEm)
        {
            return _appointmentRepository.AddAssignment(idApp, idEm);
        }

        public async Task<bool> UpdateAppointmentWithoutService(long id, Appointment appointment)
        {
            var appointmentToUpdate = _appointmentRepository.GetAppointmentByID(id);

            var currentAssign = appointment.Assignments.ToList();

            var currentEmployees = appointmentToUpdate.Assignments.Select(e => e.EmployerID).ToList();

            var employeeToRemove = currentEmployees.Except(currentAssign.Select(e => e.EmployerID));

            if (appointment.AppointmentDate != null)
            {
                appointmentToUpdate.AppointmentDate = appointment.AppointmentDate;
            }
            if (appointment.Status != null)
            {
                appointmentToUpdate.Status = appointment.Status;
            }

            foreach (var employee in employeeToRemove)
            {
                var assign = appointmentToUpdate.Assignments.FirstOrDefault(e => e.EmployerID == employee);

                if (assign != null)
                {
                    appointmentToUpdate.Assignments.Remove(assign);
                }
            }

            var employeeToAdd = currentAssign.Select(e => e.EmployerID).Except(currentEmployees).ToList();
            foreach (var employeeId in employeeToAdd)
            {
                appointmentToUpdate.Assignments.Add(new Assignment
                {
                    AppointmentID = id,
                    EmployerID = employeeId
                });
            }

            return await _appointmentRepository.UpdateAppointmentWithoutService(appointmentToUpdate);
        }

        public async Task<bool> UpdateAppointmentWithService(long id, List<long> serviceIDs)
        {
            var getChooseServiceByAppointment = await _appointmentRepository.ListService(id);

            if (getChooseServiceByAppointment == null)
            {
                await _appointmentRepository.updateServiceInAppointmentByDoctor(id, serviceIDs);
            }
            else
            {
                var currentServiceIDs = getChooseServiceByAppointment.Select(se => se.ServiceID).ToList();

                var serviceToRemove = currentServiceIDs.Except(serviceIDs).ToList();
                if (serviceToRemove.Count > 0)
                {
                    await _appointmentRepository.RemoveChooseService(id, serviceToRemove);
                }

                var serviceToAdd = serviceIDs.Except(currentServiceIDs).ToList();
                if (serviceToAdd.Count > 0)
                {
                    await _appointmentRepository.updateServiceInAppointmentByDoctor(id, serviceToAdd);
                }
            }
            var app = GetAppointmentByIdAsync(id);
            var listPrice = await _appointmentRepository.GetAllPriceService(id);
            app.Total = 0;
            foreach (var price in listPrice)
            {
                app.Total += price;
            }
            _appointmentRepository.UpdateTotalAppointment(app);
            return true;
        }
    }
}
