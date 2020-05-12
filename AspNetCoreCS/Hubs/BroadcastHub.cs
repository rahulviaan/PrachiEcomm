using Microsoft.AspNetCore.SignalR;
using ReadEdgeCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReadEdgeCore.Hubs
{
    public class BroadcastHub : Hub
    {
        public void SendMessage(string message, double percentage)
        {
            
             Clients.All.SendAsync("ReceiveMessage", message, message);
        }
        public void SendMessageBundle(string message, double percentage)
        {

            Clients.All.SendAsync("SendMessageBundle", message, message);
        }
        
    }
}
