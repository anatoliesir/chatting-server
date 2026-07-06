using ChatApp.Application.Common.Interfaces;
using ChatApp.Shared.Models;
using MediatR;

namespace ChatApp.Application.Chats.Queries
{
    public record GetAllGlobalChatQuery() : IRequest<List<GlobalChat>>;

    public class GetAllGlobalChatQueryHandler : IRequestHandler<GetAllGlobalChatQuery, List<GlobalChat>>
    {
        private readonly IGlobalChatRepository _globalChatRepository;

        public GetAllGlobalChatQueryHandler(IGlobalChatRepository globalChatRepository)
        {
            _globalChatRepository = globalChatRepository;
        }

        public async Task<List<GlobalChat>> Handle(GetAllGlobalChatQuery request, CancellationToken cancellationToken)
        {
            List<GlobalChat> chat = await _globalChatRepository.GetAllAsync();
            if (chat == null) return new List<GlobalChat>();
            return chat;
        }
    }
}
