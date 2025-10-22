namespace WebAppMVC.Views.Services
{
    public class ChatHistoryService
    {
        private readonly string _filePath = "messages.txt";
        private readonly List<string> _messages;

        public ChatHistoryService()
        {
            if (File.Exists(_filePath))
                _messages = File.ReadAllLines(_filePath).ToList();
            else
                _messages = new List<string>();
        }

        public IReadOnlyList<string> GetAllMessages() => _messages;

        public void AddMessage(string message)
        {
            _messages.Add(message);
            File.AppendAllLines(_filePath, new[] { message });
        }
    }
}
