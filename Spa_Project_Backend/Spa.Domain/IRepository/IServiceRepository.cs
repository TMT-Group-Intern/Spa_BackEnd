
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IServiceRepository
    {
        ServiceEntity CreateServiceEntitys(ServiceEntity ServiceEntity);
    
        IEnumerable<ServiceEntity> GetAllServiceEntity();

        Task<ServiceEntity> GetLastServiceEntityAsync();

        void UpdateServiceEntity(ServiceEntity ServiceEntity);

        Task<bool> DeleteServiceEntity(ServiceEntity ServiceEntity);

        ServiceEntity GetServiceEntityById(long id);

        Task<ServiceEntity> GetServiceEntityByPhone(string phone);
    }
}
