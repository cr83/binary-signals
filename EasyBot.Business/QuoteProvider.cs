using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.Globalization;

using EasyBot.Models;
using EasyBot.Models.Enums;

using jsonapi;
using jsonapi.response;

namespace EasyBot.Business
{
    public class QuoteProvider
    {
        private bool _isInvalidLogin;
        private JSONApi _jsonApi;
        private bool _isConnected;
        private string[] _symbols;
        private readonly string _username;
        private readonly string _password;
        private bool _doNotReconnect;

        public event EventHandler<QuoteProviderStatusEventArgs> QuoteProviderStatusChanged;

        public event EventHandler<QuoteUpdatedEventArgs> QuoteUpdated;

        public QuoteProvider(string[] symbols, string username, string password)
        {
            this._symbols = symbols;
            this._username = username;
            this._password = password;
            this._jsonApi = new JSONApi();
            this._jsonApi.OnConnected += new ConnectedEventHandler(this._jsonApi_OnConnected);
            this._jsonApi.OnStatusChanged += new StatusChangedEventHandler(this._jsonApi_OnStatusChanged);
            this._jsonApi.OnStreamingUpdate += new StreamingUpdateEventHandler(this._jsonApi_OnStreamingUpdate);
            this._jsonApi.OnDisconnected += new DisconnectedEventHandler(this._jsonApi_OnDisconnected);
            this._jsonApi.DefaultDataProvider = "netdania_fxa";
            this._jsonApi.setConnectParams("http://balancer.netdania.com/StreamingServer/StreamingServer", this._username, this._password);
            this._jsonApi.selectProtocol((byte)1);
            this._jsonApi.setPollingInterval(500);
            this._jsonApi.Connect();
        }

        private void _jsonApi_OnDisconnected(DisconnectReason reason)
        {
            this._isConnected = false;
            if (!_doNotReconnect)
             Task.Factory.StartNew(new Action(this.Reconnect));
        }

        private void Reconnect()
        {
            Task.Delay(10000).Wait();
            //Thread.Sleep(10000);
            if (this._isConnected)
                return;
            this._jsonApi = (JSONApi)null;
            GC.Collect();
            this._jsonApi = new JSONApi();
            this._jsonApi.OnConnected += new ConnectedEventHandler(this._jsonApi_OnConnected);
            this._jsonApi.OnStatusChanged += new StatusChangedEventHandler(this._jsonApi_OnStatusChanged);
            this._jsonApi.OnStreamingUpdate += new StreamingUpdateEventHandler(this._jsonApi_OnStreamingUpdate);
            this._jsonApi.OnDisconnected += new DisconnectedEventHandler(this._jsonApi_OnDisconnected);
            this._jsonApi.DefaultDataProvider = "netdania_fxa";
            this._jsonApi.setConnectParams("http://balancer.netdania.com/StreamingServer/StreamingServer", this._username, this._password);
            this._jsonApi.selectProtocol((byte)1);
            this._jsonApi.setPollingInterval(500);
            this._jsonApi.Connect();
        }

        //public void Connect(string[] symbols)
        //{
        //    this._symbols = symbols;
        //    this._jsonApi.Connect();
        //}

        public void Disconnect()
        {
            if (!this._isConnected)
                return;
            _doNotReconnect = true;
            this._jsonApi.Disconnect();
        }

        private void _jsonApi_OnStreamingUpdate(MonitorPriceResponse resObject)
        {
            if (this.QuoteUpdated == null)
                return;
            this.QuoteUpdated((object)this, new QuoteUpdatedEventArgs(resObject.get((short)25), double.Parse(resObject.get((short)11), NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), double.Parse(resObject.get((short)10), NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), double.Parse(resObject.get((short)14), NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), double.Parse(resObject.get((short)15), NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture)));
        }

        private void _jsonApi_OnStatusChanged(string message)
        {
            if (!this._isInvalidLogin && message.ToLower().Contains("invalid user-group"))
                this.ShowInvalidAccountDataMessage();
            if (this.QuoteProviderStatusChanged == null)
                return;
            this.QuoteProviderStatusChanged((object)this, new QuoteProviderStatusEventArgs(message));
        }

        private void ShowInvalidAccountDataMessage()
        {
            this._isInvalidLogin = true;
            //Settings.Default.UserName = "";
            //Settings.Default.Password = "";
            //Settings.Default.Save();
            //this._mainWindow.Dispatcher.BeginInvoke((Delegate)(() =>
            //{
            //    int num = (int)MessageBox.Show(this._mainWindow, "Invalid username or password. Please restart application.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            //    Environment.Exit(0);
            //}));

            throw new Exception("Invalid username or password. Please restart application.");
        }

        private void _jsonApi_OnConnected(string message)
        {
            this._isConnected = true;
            this._jsonApi.monitorPrice(this._symbols);
            this._jsonApi.PrintRequestList();
        }

        public List<Candle> GetHistory(string symbol, TimeFrame timeFrame)
        {
            List<Candle> list = new List<Candle>();
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://balancer.netdania.com/StreamingServer/StreamingServer?xml=chart&group={0}&pass={1}&source=netdania_fxa&symbol={2}&fields=101|102|103|104|&time=dd MMM yyyy HH:mm:ss&max=200&scale={3}&tzone=GMT", (object)this._username, (object)this._password, (object)symbol, (object)TimeFrameConverter.ToMinutes(timeFrame)));
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(httpWebRequest.GetResponse().GetResponseStream());
                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//point");
                if (xmlNodeList != null)
                {
                    foreach (XmlLinkedNode xmlLinkedNode in xmlNodeList)
                    {
                        if (xmlLinkedNode.Attributes != null)
                            list.Add(new Candle(double.Parse(xmlLinkedNode.Attributes["f101"].Value, NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), double.Parse(xmlLinkedNode.Attributes["f102"].Value, NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), double.Parse(xmlLinkedNode.Attributes["f103"].Value, NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), double.Parse(xmlLinkedNode.Attributes["f104"].Value, NumberStyles.Any, (IFormatProvider)CultureInfo.InvariantCulture), DateTime.Parse(xmlLinkedNode.InnerText, (IFormatProvider)CultureInfo.InvariantCulture)));
                    }
                }
            }
            catch (Exception ex)
            {
                if (!this._isInvalidLogin)
                {
                    if (ex.Message.ToLower().Contains("forbidden"))
                        this.ShowInvalidAccountDataMessage();
                }
            }
            return list;
        }
    }
}
