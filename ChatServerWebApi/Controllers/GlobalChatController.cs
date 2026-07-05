using ChatServerWebApi.Models;
using ChatApp.Shared.Models;
using ChatServerWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalChatController : ControllerBase
    {
        private readonly IGlobalChatRepository _globalChatRepository;

        public GlobalChatController(IGlobalChatRepository globalChatRepository)
        {
            _globalChatRepository = globalChatRepository;
        }


        // GET: api/<GlobalChatController>
        [HttpGet]
        public async Task<List<GlobalChat>> Get()
        {
            return await _globalChatRepository.GetAllAsync();
        }
    }
}
