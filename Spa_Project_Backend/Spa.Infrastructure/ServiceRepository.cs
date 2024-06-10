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
    public class ServiceRepository : EfRepository<ServiceEntity>,IServiceRepository
    {
        public ServiceRepository(SpaDbContext spaDbContext) : base(spaDbContext)
        {
        }

        public ServiceEntity CreateServiceEntitys(ServiceEntity serviceEntity)
        {
            Add(serviceEntity);
            return serviceEntity;
        }

        public async Task<bool> DeleteServiceEntity(ServiceEntity serviceEntity)
        {
            if (serviceEntity != null)
            {
                // customer.IsActive = false;
                // Update(customer);
                 DeleteById(serviceEntity);
                return true;
            }
            return false;
        }

        public IEnumerable<ServiceEntity> GetAllServiceEntity()
        {
            return GetAll();
        }

        public async Task<ServiceEntity> GetLastServiceEntityAsync()
        {
            try
            {
                return await _spaDbContext.Services.OrderByDescending(s => s.ServiceCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ServiceEntity GetServiceEntityById(long idService)
        {
            return GetById(idService);
        }

        public Task<ServiceEntity> GetServiceEntityByPhone(string phone)
        {
            throw new NotImplementedException();
        }

        public void UpdateServiceEntity(ServiceEntity ServiceEntity)
        {
           Update(ServiceEntity);
        }
    }
}
