using MediatR;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using Spa.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateBillCommand : IRequest<long>
    {
        public long? CustomerID { get; set; }
        public long AppointmentID { get; set; }
        public DateTime? Date { get; set; }
        public string? BillStatus { get; set; }
        public string? Doctor { get; set; }
        public string? TechnicalStaff { get; set; }

        public double? TotalAmount { get; set; }   // tổng tiền
        public double? AmountInvoiced { get; set; } = 0;// thanh toán
        public double? AmountResidual { get; set; } = 0; // còn lại

  

        public ICollection<BillItem>? BillItems { get; set; }
    }

    public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, long>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IBillService _billService;

        public CreateBillCommandHandler(IBillService billService, IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
            _billService = billService;
        }
        public async Task<long> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            long idNewBill = 0;
            Bill bill = new Bill()
            {
                AppointmentID = request.AppointmentID,
                AmountResidual = request.TotalAmount,
                AmountInvoiced = request.AmountInvoiced,
                TechnicalStaff = request.TechnicalStaff,
                Doctor = request.Doctor,
                TotalAmount = request.TotalAmount,
                // BillItems = request.BillItems,
                BillStatus = request.BillStatus,
                CustomerID = request.CustomerID,
                Date = DateTime.Now,
            
            };
            var newBill = await _billService.CreateBill(bill);
            if (newBill != null)
            {
                var checkChooservice = _appointmentService.GetAppointmentByIdAsync(request.AppointmentID); // check service trong appointment
                var newBillInDatabase = await _billService.GetNewBillAsync();
                idNewBill = newBillInDatabase.BillID;
                if (checkChooservice.ChooseServices != null)
                {
                    List<BillItem> newBillItems = new List<BillItem>();
                    bill.TotalAmount = 0;
                    bill.AmountResidual = 0;
                    foreach (var item in checkChooservice.ChooseServices)
                    {
                        bill.TotalAmount += item.Service.Price;
                        bill.AmountResidual += item.Service.Price;
                        newBillItems.Add(new BillItem()
                        {
                            BillID = newBillInDatabase.BillID,
                            ServiceID = item.ServiceID,
                            Quantity = 1, // đổi chooseService thêm trường quatity rồi chèn vào đây
                            ServiceName = item.Service.ServiceName,
                            UnitPrice = item.Service.Price,
                        });
                    }
                    await _billService.AddBillItem(newBillItems);
                }
            }

            return idNewBill;
        }
    }
}
