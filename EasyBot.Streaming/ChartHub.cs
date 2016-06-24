using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;

namespace EasyBot.Streaming
{
    //[Authorize]
    public class ChartHub : Hub
    {
        //public void HistoryUpdate(string connectionId, object data)
        //{
        //    Clients.All.historyUpdate(data);
        //}

        //public void RisedSignal(string connectionId, object data)
        //{
        //    Clients.All.risedSignal(data);
        //}
        //public void SetInstrumentSettings(string symbol, string timeFrame, decimal range)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SubscribeOnHistory(string symbol, string timeFrame)
        //{
        //    Streamer.GetOrCreate(Context.ConnectionId).SubscribeOnHistory(symbol, timeFrame);
        //}

        //public override Task OnConnected()
        //{
        //    Streamer.GetOrCreate(Context.ConnectionId);
        //    return base.OnConnected();
        //}

        //public override Task OnReconnected()
        //{
        //    Streamer.GetOrCreate(Context.ConnectionId);
        //    return base.OnReconnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    Streamer.Remove(Context.ConnectionId);
        //    return base.OnDisconnected(stopCalled);
        //}
    }
}
