using ChatApp.Application.Chats.Commands;
using ChatApp.Application.Chats.Queries;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ChatServerWebApi.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatHub> _hubContext;

        public GlobalChatController(IMediator mediator, IHubContext<ChatHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }


        // GET: api/<GlobalChatController>
        [HttpGet]
        public async Task<List<GlobalChat>> Get()
        {
            var query = new GetAllGlobalChatQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpDelete("clear-all")]
        public async Task<IActionResult> ClearChat([FromQuery] string username)
        {
            try
            {
                var command = new DeleteAllGlobalChatCommand(username);
                await _mediator.Send(command);

                await _hubContext.Clients.All.SendAsync("ChatCleared");

                return Ok("All messages have been deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
    }
}
