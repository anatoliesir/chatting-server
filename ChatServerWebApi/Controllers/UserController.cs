using ChatApp.Application.Users.Commands;
using ChatApp.Application.Users.Queries;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        // We no longer inject the repository here! Only MediatR.
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto registerDto)
        {
            try
            {
                // Pack data into the Command envelope
                var command = new RegisterUserCommand(registerDto.Username, registerDto.Password);

                // Send it through MediatR pipeline
                var result = await _mediator.Send(command);

                return Ok(result); // Returns "User registered successfully!"
            }
            catch (ArgumentException ex)
            {
                // Catches the 'throw new' from our handler and returns a 400 Bad Request
                return BadRequest(ex.Message);
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Pack data into the Query envelope
            var query = new LoginUserQuery(loginDto.Username, loginDto.Password);

            // MediatR finds the handler and returns a true/false boolean
            bool isSuccess = await _mediator.Send(query);

            if (!isSuccess)
                return Unauthorized("Invalid username or password!");

            return Ok("User logged in successfully!");
        }

        // DELETE: api/User/delete/{username}
        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            // Pack data into the Delete Command envelope
            var command = new DeleteUserCommand(username);

            bool isDeleted = await _mediator.Send(command);

            if (!isDeleted)
                return NotFound("User not found!");

            return Ok("Account deleted successfully!");
        }
    }
}