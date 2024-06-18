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
            //  Add(appointment);
            //Assignment assignment = new Assignment
            //{
            //    AppointmentID = appointment.AppointmentID,
            //    EmployerID = appointment.Assignments.Select(e => e.EmployerID),
            //};
            // _spaDbContext.Assignments.Add(appointment.Assignments);
            Add(appointment);
            return appointment;
        }

        public async Task<bool> UpdateAppointmentWithoutService(Appointment appointment)
        {
            _spaDbContext.UpdateRange(appointment);
            _spaDbContext.SaveChangesAsync();
            return true;
        }

        public Assignment CreateAssignment(Assignment assignment)
        {
            _spaDbContext.Assignments.Add(assignment);
            return assignment;
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
                                .Include(a => a.Assignments).ThenInclude(e => e.Employees)
                                 .Include(s => s.ChooseServices).ThenInclude(se => se.Service).ToList();
        }

        public Appointment GetAppointmentByID(long appointmentId)
        {
            return _spaDbContext.Appointments.Where(a => a.AppointmentID == appointmentId)
                                             .Include(c => c.Customer)
                                             .Include(e => e.Assignments).ThenInclude(em => em.Employees)
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

        public async Task<bool> updateServiceInAppointmentByDoctor(long id, List<long> serviceID)
        {
          //  ChooseService chooseservice = null;
            foreach (var i in serviceID)
            {
                ChooseService chooseservice = new ChooseService
                {
                    AppointmentID = id,
                    ServiceID = i
                };
             await _spaDbContext.AddAsync(chooseservice);
             await _spaDbContext.SaveChangesAsync();
            }
            return true;
        }
            
    

        public async Task<bool> AddAssignment(long idApp, long idEm)
        {
            Assignment assignment = new Assignment
            {
                AppointmentID = idApp,
                EmployerID = idEm
            };
            await _spaDbContext.AddAsync(assignment);
            await _spaDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChooseService>> ListService(long id)
        {
            return await _spaDbContext.ChooseServices.Where(a => a.AppointmentID == id).ToListAsync();
        }

        public async Task RemoveChooseService(long id, List<long> serviceIDs)
        {
            foreach (var serviceID in serviceIDs)
            {
                var chooseservice = await _spaDbContext.ChooseServices
                    .FirstOrDefaultAsync(cs => cs.AppointmentID == id && cs.ServiceID == serviceID);

                if (chooseservice != null)
                {
                    _spaDbContext.ChooseServices.Remove(chooseservice);
                }
            }
            await _spaDbContext.SaveChangesAsync();
        }
    }
}
