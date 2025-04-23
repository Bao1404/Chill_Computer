using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Chill_Computer.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Chill_Computer.ViewModels;

namespace Chill_Computer.Helpers
{
    public class ChatHub : Hub
    {
        private readonly ChillComputerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;
        private static Dictionary<string, string> userRoles = new();
        public ChatHub(ChillComputerContext context, IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task RegisterFromSession()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.Session == null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "Session is not available.");
                return;
            }

            var userId = httpContext.Session.GetObject<int>("_userId");
            var userRole = httpContext.Session.GetObject<string>("_userRole");

            if (userId == null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "Session expired or not logged in.");
                return;
            }

            var account = _accountService.GetAccountByUserId(userId);

            if (account == null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "Account not found.");
                return;
            }

            var roleName = userRole;

            // Lưu roleName cho connectionId
            userRoles[Context.ConnectionId] = roleName;
        }


        public async Task SendMessage(string message)
        {
            if (!userRoles.TryGetValue(Context.ConnectionId, out var senderRole))
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "You are not registered.");
                return;
            }

            IEnumerable<string> receivers;

            if (senderRole == "Customer")
            {
                receivers = userRoles
                    .Where(x => x.Value == "Employee")
                    .Select(x => x.Key);
            }
            else if (senderRole == "Employee")
            {
                receivers = userRoles
                    .Where(x => x.Value == "Customer")
                    .Select(x => x.Key);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "System", "Invalid role.");
                return;
            }

            // Lưu tin nhắn vào session
            SaveChatToSession(message, senderRole);

            // Gửi lại cho người gửi
            await Clients.Caller.SendAsync("ReceiveMessage", senderRole, message);

            // Gửi đến các người nhận
            foreach (var receiverId in receivers)
            {
                await Clients.Client(receiverId).SendAsync("ReceiveMessage", senderRole, message);
            }
        }




        public override Task OnDisconnectedAsync(Exception? exception)
        {
            userRoles.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public void SaveChatToSession(string message, string role)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var chatHistory = session?.GetObject<List<ChatMessage>>("ChatHistory") ?? new List<ChatMessage>();

            chatHistory.Add(new ChatMessage
            {
                Role = role,
                Message = message,
                Timestamp = DateTime.Now
            });

            session?.SetObject("ChatHistory", chatHistory);
        }

        public async Task LoadChatHistory()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var chatHistory = session?.GetObject<List<ChatMessage>>("ChatHistory");

            if (chatHistory != null)
            {
                foreach (var chat in chatHistory)
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", chat.Role, chat.Message);
                }
            }
        }


    }

}
