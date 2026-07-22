using Microsoft.AspNetCore.SignalR;
using ChatApp.Application.Common.Interfaces;
using ChatApp.Domain.Entities;

namespace ChatServerWebApi.Hubs
{
    // Moștenim clasa Hub din SignalR
    public class ChatHub : Hub
    {
        private readonly IGlobalChatRepository _globalChatRepository;

        public ChatHub(IGlobalChatRepository globalChatRepository)
        {
            _globalChatRepository = globalChatRepository;
        }

        // This message will be called from the interface app(Blazor, etc.)
        public async Task SendMessage(string userName, string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText)) return;

            var noulMesaj = new GlobalChat
            {
                UserName = userName,
                Message = messageText,
                SentAt = DateTime.UtcNow 
            };
            await _globalChatRepository.AddAsync(noulMesaj);

            // If the message count surpasses the limit set in here, then it will proceed to delete the oldest messages
            await _globalChatRepository.TrimMessagesAsync(100);

            // SignalR Magic: Send to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", noulMesaj);
        }
    }
}