using ChatApp.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Chats.Commands
{
    public record DeleteAllGlobalChatCommand(string RequesterUsername) : IRequest;

    public class DeleteAllGlobalChatCommandHandler : IRequestHandler<DeleteAllGlobalChatCommand>
    {
        private readonly IGlobalChatRepository _globalChatRepository;

        public DeleteAllGlobalChatCommandHandler(IGlobalChatRepository globalChatRepository)
        {
            _globalChatRepository = globalChatRepository;
        }

        public async Task Handle(DeleteAllGlobalChatCommand command, CancellationToken cancellationToken)
        {
            if (command.RequesterUsername != "admin")
            {
                throw new UnauthorizedAccessException("Only the admin can delete the global chat!");
            }

            await _globalChatRepository.DeleteAllAsync();

            await _globalChatRepository.GetAllAsync();
        }
    }

}
