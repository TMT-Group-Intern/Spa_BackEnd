using MediatR;
using Spa.Application.Models;
using Spa.Domain.Entities;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Commands
{
    public class CreateServiceCommand :IRequest<long>
    {
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }

    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, long>
    {
        private readonly IServiceService _serviceService;

        public CreateServiceCommandHandler(IServiceService customerService)
        {
            _serviceService = customerService;
        }
        public async Task<long> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var serviceNew = new ServiceEntity
            {             
                ServiceName = request.ServiceName,  
                Description = request.Description,
                Price = request.Price               
            };
           await _serviceService.CreateService(serviceNew);
            return serviceNew.ServiceID;
        }
    }


}
