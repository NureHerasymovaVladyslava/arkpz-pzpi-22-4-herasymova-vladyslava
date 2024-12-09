using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class AlertHub : Hub
    {
        public const string ReceiveAlertString = "ReceiveAlert";
        //public async Task SendAlert(int roomId, string alertReason)
        //{
        //    await Clients.All.SendAsync("ReceiveAlert", roomId, alertReason);
        //}
    }
}
