using ChatApp.Shared.Models;
using ChatApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using ChatApp.Application.Chats.Queries;
using ChatApp.Application.Chats.Commands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GlobalChatController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // GET: api/<GlobalChatController>
        [HttpGet]
        public async Task<List<GlobalChat>> Get()
        {
            var query = new GetAllGlobalChatQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        // DELETE: api/
        [HttpDelete("clear-all")]
        public async Task<IActionResult> ClearChat([FromBody] string username)
        {
            try
            {
                var command = new DeleteAllGlobalChatCommand(username);
                await _mediator.Send(command);
                return Ok("All messages have been deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
    }
}
