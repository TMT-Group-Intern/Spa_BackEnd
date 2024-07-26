﻿using Spa.Domain.Entities;
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

        Task<bool> UpdateAppointmentWithService(long id,List<long> serviceID, string status, string? notes);

        Task<bool> UpdateStatus(long id, string status);

        Task<bool> UpdateDiscount(long id, int perDiscount);

        Task<bool> AssignTechnicalStaff(long idApp, long idEm);

        Task<bool> UpdateAppointment(long idApp, Appointment appointment);

        Task<IEnumerable<Appointment>> getAppointmentPage(long idBranch, int pageNumber, int pageSize);
        Task<List<Appointment>> GetAppointmentFromDayToDay(long brancdID, DateTime fromDate, DateTime toDate);

        Task<Appointment> GetDetailAppointmentToCreateBill(long appointmentID);

        Task<int> GetAllItem();
    }
}
