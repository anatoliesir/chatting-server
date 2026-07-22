using ChatApp.Application.Common.Interfaces;
using ChatApp.Domain.Entities;
using MediatR;

namespace ChatApp.Application.Users.Commands
{
    // The Command defines what data we need to execute the registration
    public record RegisterUserCommand(string Username, string Password) : IRequest<string>;

    // The Handler contains the actual business logic that was previously in the controller
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Enter username and password!");

            var existingUser = await _userRepository.GetByUsernameAsync(request.Username.Trim());
            if (existingUser != null)
                throw new ArgumentException("Username is already taken!");

            // Hash the password inside the core application logic layer
            string securedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newAccount = new User
            {
                Id = 0, // Triggers EF Core auto-increment feature natively
                UserName = request.Username.Trim(),
                PasswordHash = securedPassword
            };

            await _userRepository.AddAsync(newAccount);

            return "User registered successfully!";
        }
    }
}