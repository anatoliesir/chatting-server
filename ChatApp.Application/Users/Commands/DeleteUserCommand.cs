using ChatApp.Application.Common.Interfaces;
using MediatR;

namespace ChatApp.Application.Users.Commands
{
    // The Command requests account deletion based on a specific username
    public record DeleteUserCommand(string Username) : IRequest<bool>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                return false;

            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
                return false; // User does not exist, operation cannot proceed

            // Call the abstraction to delete the core entity tracking instance
            await _userRepository.DeleteAsync(user.Id);
            return true;
        }
    }
}