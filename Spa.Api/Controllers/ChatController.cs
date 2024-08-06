using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Spa.Domain.Service;

namespace Spa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly MessageService _messageService;

        public ChatController(MessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpGet]
        public async Task<ActionResult> GetMessage()
        {
           var listMess = await _messageService.GetMessagesAsync();
            return new JsonResult(listMess);
        }
    }
}
