using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class MessageRepository
    {
        private readonly SpaDbContext _spaDbContext;

        public MessageRepository(SpaDbContext spaDbContext)
        {

            _spaDbContext = spaDbContext;
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _spaDbContext.Messages.ToListAsync();
        }

        public async Task<bool> AddMessagesAsync(Message message)
        {
            await _spaDbContext.Messages.AddAsync(message);
            return true;
        }

    }
}
