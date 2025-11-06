using Microsoft.AspNetCore.SignalR;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC.Views.Services;
using WebAppMVC_Models;

namespace WebAppMVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatHistoryService chatHistory;
        IChatRepository chatRepository;
        readonly IApplicationUserRepository applicationUserRepository;

        public ChatHub(ChatHistoryService chatHistory, IChatRepository chatRepository, IApplicationUserRepository applicationUserRepository)
        {
            this.chatHistory = chatHistory;
            this.chatRepository = chatRepository;
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task SendMessage(string user, string message)
        {
            //string formatted = $"{user}: {message}";
            //chatHistory.AddMessage(formatted);
            
            var userClaim = Context.User.Claims.FirstOrDefault();
            var userAppl = applicationUserRepository.FirstOrDefault(q => q.Id == userClaim.Value);
            var chatModel = new ChatModel();

            chatModel.ApplicationUserId = userAppl.Id;
            chatModel.MessageUserNickName = user;
            chatModel.MessageText = message;
            
            chatRepository.Add(chatModel);
            chatRepository.Save();

            await Clients.All.SendAsync("ReceiveMessage", user, $"{chatModel.MessageDate.ToShortDateString()} {chatModel.MessageDate.ToShortTimeString()} : {message}");
        }

        public async Task LoadMessages()
        {
            string[]? parts;

            parts = new string[] { "-", "-" };
            
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "Загрузка данных из файла messages.txt");
            //инитим из текстового файла
            foreach (var msg in chatHistory.GetAllMessages())
            {
                parts = msg.Split(':', 2);
                if (parts.Length == 2)
                    await Clients.Caller.SendAsync("ReceiveMessage", parts[0], parts[1].Trim());
            }

            //добавляем из базы
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "Загрузка данных из MS SQL");
            var messagesFromDb = chatRepository.GetAll(includeProperties:"ApplicationUser", orderBy: q => q.OrderBy(m => m.MessageDate));
            foreach(var msg in messagesFromDb) 
            {
                parts[0] = msg.MessageUserNickName;
                parts[1] = msg.MessageText;
                if (parts.Length == 2)
                    await Clients.Caller.SendAsync("ReceiveMessage", parts[0], $"{msg.MessageDate.ToShortDateString()} {msg.MessageDate.ToShortTimeString()} : {parts[1].Trim()}");
            }
        }




    }
}
