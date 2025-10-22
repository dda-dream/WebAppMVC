using Microsoft.AspNetCore.SignalR;
using WebAppMVC.Views.Services;

namespace WebAppMVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatHistoryService _chatHistory;

        public ChatHub(ChatHistoryService chatHistory)
        {
            _chatHistory = chatHistory;
        }

        public async Task SendMessage(string user, string message)
        {
            string formatted = $"{user}: {message}";
            _chatHistory.AddMessage(formatted);

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task LoadMessages()
        {
            foreach (var msg in _chatHistory.GetAllMessages())
            {
                var parts = msg.Split(':', 2);
                if (parts.Length == 2)
                    await Clients.Caller.SendAsync("ReceiveMessage", parts[0], parts[1].Trim());
            }
        }
    }
}
