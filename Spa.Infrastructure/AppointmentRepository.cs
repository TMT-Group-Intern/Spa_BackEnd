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

        public Appointment CreateAppointment(Appointment customer)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAppointment(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointment()
        {

            //         return _spaDbContext.Appointments
            //.Join(_spaDbContext.Customers,
            //    appointment => appointment.CustomerID,
            //    customer => customer.CustomerID,
            //    (appointment, customer) => new { Appointment = appointment, Customer = customer })
            //.Join(_spaDbContext.Employees,
            //    combined => combined.Appointment.EmployeeID,
            //    employee => employee.EmployeeID,
            //    (combined, employee) => new { combined.Appointment, combined.Customer, Employee = employee })
            //.Join(_spaDbContext.Branches,
            //    combined => combined.Appointment.BranchID,
            //    branch => branch.BranchID,
            //    (combined, branch) => new Appointment
            //    {
            //        AppointmentID = combined.Appointment.AppointmentID,
            //        AppointmentDate = combined.Appointment.AppointmentDate,
            //        Customer = combined.Customer,
            //        Employee = combined.Employee,
            //        Branch = branch,
            //        ChooseServices = combined.Appointment.ChooseServices
            //    })
            //.ToList();
            return await _spaDbContext.Appointments.Include(c => c.Customer)
                                              .Include(e => e.Employee)
                                               .Include(b => b.Branch)
                                              .ToListAsync();
        }

        public void UpdateAppointment(Appointment customer)
        {
            throw new NotImplementedException();
        }
    }
}
