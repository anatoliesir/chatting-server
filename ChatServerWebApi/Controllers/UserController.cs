using ChatServerWebApi.Models;
using ChatServerWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Shared.Models;
using BCrypt.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        private const string DummyHash = "$2a$11$HBMH6EubvSmwO0vTGWK9OORXQz7eLq8b8e0tMh/SgS7vX2YhW8C2.";
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Username) || string.IsNullOrWhiteSpace(registerDto.Password))
                return BadRequest("Enter username and password!");

            var existingUser = await _userRepository.GetByUsernameAsync(registerDto.Username);
            if (existingUser != null)
                return BadRequest("Username is already taken!");

            string securedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newAccount = new User
            {
                Id = 0,
                UserName = registerDto.Username,
                PasswordHash = securedPassword
            };

            await _userRepository.AddAsync(newAccount);
            return Ok("User registered successfully!");
        }


        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Enter username and password!");

            User? user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            string hashForVerification = user?.PasswordHash ?? DummyHash;
            bool verificationHash = BCrypt.Net.BCrypt.Verify(loginDto.Password, hashForVerification);
            if (user == null || !verificationHash) return Unauthorized("Invalid username or password!");

            return Ok("User logged in successfully!");
        }

        // DELETE: api/User/delete/{username}
        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username cannot be empty!");

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return NotFound("User not found!");

            // Note: Ensure your IUserRepository and UserRepository implement a Delete/Remove method
            await _userRepository.DeleteAsync(user.Id);

            return Ok("Account deleted successfully!");
        }
    }
}
