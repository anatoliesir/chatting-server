using ChatApp.Application.Common.Interfaces;
using MediatR;

namespace ChatApp.Application.Users.Queries
{
    // The Query asks the system to validate the credentials and returns true/false
    public record LoginUserQuery(string Username, string Password) : IRequest<bool>;

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        // Dummy hash prevents timing attacks by processing invalid users at the same speed
        private const string DummyHash = "$2a$11$HBMH6EubvSmwO0vTGWK9OORXQz7eLq8b8e0tMh/SgS7vX2YhW8C2.";

        public LoginUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return false;

            var user = await _userRepository.GetByUsernameAsync(request.Username.Trim());
            string hashForVerification = user?.PasswordHash ?? DummyHash;

            // Verify the plain text password against the secured hash
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, hashForVerification);

            if (user == null || !isPasswordValid)
                return false;

            return true;
        }
    }
}