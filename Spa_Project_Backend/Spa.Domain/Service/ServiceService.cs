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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public async Task CreateService(ServiceEntity serviceEntity)
        {
            var lastSerID = await GenerateServiceCodeAsync();

            serviceEntity.ServiceCode = lastSerID;
            _serviceRepository.CreateServiceEntitys(serviceEntity);
        }

        public Task<bool> DeleteService(long ServiceId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateServiceCodeAsync()
        {
            var lastServiceCode = await _serviceRepository.GetLastServiceEntityAsync();

            if (lastServiceCode == null)
            {
                return "DV0001";
            }
            var lastCode = lastServiceCode.ServiceCode;
            int numericPart = int.Parse(lastCode.Substring(2));
            numericPart++;
            return "DV" + numericPart.ToString("D4");
        }

        public IEnumerable<ServiceEntity> GetAllService()
        {
            return _serviceRepository.GetAllServiceEntity();
        }

        public ServiceEntity GetServiceById(long id)
        {
            throw new NotImplementedException();
        }

        public bool isExistService(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateService(long serviceId, ServiceEntity serviceEntity)
        {
            throw new NotImplementedException();
        }
    }
}
