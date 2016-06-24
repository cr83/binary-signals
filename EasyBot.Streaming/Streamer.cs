using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using EasyBot.Business;
using EasyBot.Models;
using EasyBot.Common;

using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;

using log4net;

namespace EasyBot.Streaming
{
    public class Streamer : IDisposable
    {
        #region fields
        private ILog _log = LogManager.GetLogger(typeof(Streamer));

        private readonly IHubContext _hubContext;
        QuoteProvider _quoteProvider;
        List<Instrument> _instruments;
        private static Dictionary<string, Streamer> _streamers = new Dictionary<string, Streamer>();
        string _connectionId;
        string _symbol;
        private bool _disposed;
        #endregion fields

        #region static
        public static void Remove()
        {
            var sessionId = HttpContext.Current.Session.SessionID;
            lock (SessionMutex.Get(sessionId))
            {
                Streamer streamer;
                _streamers.TryGetValue(sessionId, out streamer);
                if (streamer != null)
                {
                    _streamers.Remove(sessionId);
                    streamer.Dispose();
                }
            }
            SessionMutex.Remove(sessionId);
        }

        public static void RemoveAll()
        {
            foreach (var item in _streamers)
            {
                item.Value.Dispose();
            }
            _streamers.Clear();
        }

        public static Streamer GetOrCreate(string connectionId)
        {
            var sessionId = HttpContext.Current.Session.SessionID;
            lock (SessionMutex.Get(sessionId))
            {
                Streamer streamer;
                _streamers.TryGetValue(sessionId, out streamer);
                if (streamer == null)
                {
                    streamer = new Streamer(connectionId, AppGlobal.Container.Resolve<QuoteProvider>());
                    _streamers.Add(sessionId, streamer);
                }
                else
                {
                    streamer._connectionId = connectionId;
                }
                return streamer;
            }
        }
        #endregion static

        #region ctor
        public Streamer(string connectionId, QuoteProvider quoteProvider)
        {
            this._quoteProvider = quoteProvider;
            this._instruments = Config.GetInstruments(this._quoteProvider);
            this._connectionId = connectionId;

            SubscribeOnInstrumetsEvents();

            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ChartHub>();

            _log.Info(String.Format("Streamer created. ConnectionId: {0}", _connectionId));
        }
        #endregion ctor

        #region private
        private void SubscribeOnInstrumetsEvents()
        {
            foreach (Instrument instrument in this._instruments)
            {
                instrument.HistoryUpdated += new EventHandler<HistoryEventArgs>(this.InstrumentHistoryUpdated);
                instrument.RisedSignal += new EventHandler<SignalEventArgs>(this.InstrumentRisedSignal);
                instrument.Error += new EventHandler<InstrumentErrorEventArgs>(this.InstrumentError);
                //if (instrument.Symbol != "AUD/USD") // TODO: Remove this
                {
                    instrument.Available = true;
                    instrument.Start();
                }
            }
        }

        private void InstrumentError(object sender, InstrumentErrorEventArgs e)
        {
            _log.Error(e.Message);
            _hubContext.Clients.Client(_connectionId).errorOccured(e.Symbol);
        }

        private void InstrumentRisedSignal(object sender, SignalEventArgs e)
        {
            ThrowIfDisposed();
            _hubContext.Clients.Client(_connectionId).risedSignal(e.Signal);
            //this.Dispatcher.BeginInvoke((Delegate)(() =>
            //{
            //    this._signals.Insert(0, e.Signal);
            //    if (!this.IsActive)
            //    {
            //        ++this._newSignals;
            //        this.TaskbarItemInfo.Overlay = (ImageSource)BitmapGenerator.GenerateFromInt(this._newSignals);
            //        MainWindow.FlashWindow(new WindowInteropHelper((Window)this).Handle, true);
            //    }
            //    try
            //    {
            //        this._media.Source = new Uri("Chime.mp3", UriKind.Relative);
            //        this._media.Play();
            //    }
            //    catch
            //    {
            //    }
            //}));
        }

        private void InstrumentHistoryUpdated(object sender, HistoryEventArgs e)
        {
            ThrowIfDisposed();
            if (e.Symbol != _symbol)
            {
                return;
            }
            _hubContext.Clients.Client(_connectionId).historyUpdate(new { e.Symbol, e.TimeFrame, Candles = e.Candles.Skip(e.Candles.Count - 30).ToList() });
            //lock (this._lock)
            //{
            //    if (e == null)
            //    {
            //        Instrument instrument = (Instrument)this.cboxInstruments.SelectedItem;
            //        // ISSUE: variable of a compiler-generated type
            //        MainWindow.\u003C\u003Ec__DisplayClass13 local_3;
            //        Task.Factory.StartNew((Action)(() =>
            //        {
            //            // ISSUE: variable of a compiler-generated type
            //            MainWindow.\u003C\u003Ec__DisplayClass13 cDisplayClass13 = local_3;
            //            List<Candle> candles = this._quoteProvider.GetHistory(instrument.Symbol, instrument.TimeFrame);
            //            if (candles.Count <= 0)
            //                return;
            //            this.Dispatcher.BeginInvoke((Delegate)(() =>
            //            {
            //                // ISSUE: reference to a compiler-generated field
            //                cDisplayClass13.\u003C\u003E4__this.chart.DataSets[0].DataContext = (object)candles;
            //                // ISSUE: reference to a compiler-generated field
            //                cDisplayClass13.\u003C\u003E4__this.chart.StartDate = Enumerable.First<Candle>((IEnumerable<Candle>)Enumerable.OrderBy<Candle, DateTime>((IEnumerable<Candle>)candles, (Func<Candle, DateTime>)(d => d.Date))).Date;
            //                // ISSUE: reference to a compiler-generated field
            //                cDisplayClass13.\u003C\u003E4__this.chart.EndDate = Enumerable.Last<Candle>((IEnumerable<Candle>)Enumerable.OrderBy<Candle, DateTime>((IEnumerable<Candle>)candles, (Func<Candle, DateTime>)(d => d.Date))).Date;
            //                // ISSUE: reference to a compiler-generated field
            //                cDisplayClass13.\u003C\u003E4__this.chart.DataSets[0].ShortTitle = instrument.Symbol;
            //            }));
            //        }));
            //    }
            //    else
            //        this.Dispatcher.BeginInvoke((Delegate)(() =>
            //        {
            //            Instrument instrument = (Instrument)this.cboxInstruments.SelectedItem;
            //            if (!(instrument.Symbol == e.Symbol) || instrument.TimeFrame != e.TimeFrame)
            //                return;
            //            if (e.Candles.Count <= 0)
            //                return;
            //            try
            //            {
            //                this.chart.DataSets[0].DataContext = (object)e.Candles;
            //                this.chart.StartDate = Enumerable.First<Candle>((IEnumerable<Candle>)Enumerable.OrderBy<Candle, DateTime>((IEnumerable<Candle>)e.Candles, (Func<Candle, DateTime>)(d => d.Date))).Date;
            //                this.chart.EndDate = Enumerable.Last<Candle>((IEnumerable<Candle>)Enumerable.OrderBy<Candle, DateTime>((IEnumerable<Candle>)e.Candles, (Func<Candle, DateTime>)(d => d.Date))).Date;
            //                this.chart.DataSets[0].ShortTitle = instrument.Symbol;
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine("");
            //            }
            //        }));
            //}
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name);
        }
        #endregion private

        public IEnumerable<Instrument> GetInstruments()
        {
            return _instruments;
        }

        public void SubscribeOnHistory(string symbol, string timeFrame)
        {
            ThrowIfDisposed();
            _symbol = symbol;
            var tf = Instrument.ParseTimeFrame(timeFrame);
            var instrument = _instruments.First(i => i.Symbol == symbol);
            if (instrument.TimeFrame != tf)
            {
                instrument.TimeFrame = tf;
            }

            List<Candle> candles = this._quoteProvider.GetHistory(symbol, tf);
            InstrumentHistoryUpdated(this, new HistoryEventArgs(symbol, tf, candles));
        }

        public void UpdateInstrumentSettings(string symbol, string timeFrame, decimal range)
        {
            ThrowIfDisposed();
            var instrument = _instruments.First(i => i.Symbol == symbol);
            instrument.Stop();
            instrument.TimeFrame = Instrument.ParseTimeFrame(timeFrame);
            instrument.Range = range;
            instrument.Start();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_quoteProvider != null)
                {
                    _quoteProvider.Disconnect();
                }
                if (_instruments != null)
                {
                    foreach (var instrument in _instruments)
                    {
                        instrument.Dispose();
                    }
                }

                _disposed = true;
            }

            _log.Info(String.Format("Streamer disposed. ConnectionId: {0}", _connectionId));
        }
    }
}
