using Chill_Computer.Contacts;
using Chill_Computer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer.Helpers
{
    public class ChatHub : Hub
    {
        private readonly ChillComputerContext _context;
        private readonly IAccountService _accountService;
        private static Dictionary<string, string> userRoles = new();
        public ChatHub(ChillComputerContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public Task RegisterRole(string roleName)
        {
            userRoles[Context.ConnectionId] = roleName;
            return Task.CompletedTask;
        }

        public Task SendMessage(string roleName, string message)
        {
            if (roleName == "customer")
            {
                var employees = userRoles
                    .Where(x => x.Value == "employee")
                    .Select(x => x.Key);

                foreach (var employeeId in employees)
                {
                    Clients.Client(employeeId).SendAsync("ReceiveMessage", roleName, message);
                }
            }
            else if (roleName == "employee")
            {
                var customers = userRoles
                    .Where(x => x.Value == "customer")
                    .Select(x => x.Key);

                foreach (var customerId in customers)
                {
                    Clients.Client(customerId).SendAsync("ReceiveMessage", roleName, message);
                }
            }

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            userRoles.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

    }
}
