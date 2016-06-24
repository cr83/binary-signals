using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using EasyBot.Models;
using EasyBot.Models.Enums;

namespace EasyBot.Business
{
    public class Instrument : IDisposable
    {
        private readonly object _locker = new object();
        private readonly Decimal _pipCost;
        private TimeFrame _timeFrame;
        private double _last;
        private Timer _timer;
        private QuoteProvider _quoteProvider;
        private bool _isRunning;
        private bool _disposed;
        private bool _isTraded;

        public bool IsTraded { get { return _isTraded; } } // TODO: Implement functional

        public bool Available { get; set; }

        public bool IsRunning { get { return _isRunning; } }

        public string Name { get; private set; }

        public string Symbol { get; private set; }

        public Decimal Range { get; set; }

        public Decimal PipCost
        {
            get { return _pipCost; }
        }

        public TimeFrame TimeFrame
        {
            get
            {
                return this._timeFrame;
            }
            set
            {
                lock (this._locker)
                {
                    this._timeFrame = value;
                    if (this._timer == null)
                        return;
                    this._timer.Stop();
                    this._timer.Interval = Convert.ToDouble((object)value);
                    this._timer.Start();
                }
            }
        }

        public double LastValue { get; private set; }

        public double Change { get; set; }

        public double ChangePercent { get; set; }

        public double Last
        {
            get
            {
                return this._last;
            }
            set
            {
                this._last = value;
                //this.OnPropertyChanged("StrLast");
                //this.OnPropertyChanged("StrChange");
                //this.OnPropertyChanged("Color");
            }
        }

        public string StrLast
        {
            get
            {
                return string.Format("{0:0.0000}", (object)this.Last);
            }
        }

        public string StrChange
        {
            get
            {
                return string.Format("{0:0.0000}({1:0.0000}%)", (object)this.Change, (object)this.ChangePercent);
            }
        }

        //public Brush Color
        //{
        //    get
        //    {
        //        if (this.Change < 0.0)
        //            return (Brush)Brushes.Red;
        //        return (Brush)Brushes.ForestGreen;
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<SignalEventArgs> RisedSignal;

        public event EventHandler<HistoryEventArgs> HistoryUpdated;

        public event EventHandler<InstrumentErrorEventArgs> Error;

        public Instrument(string name, string symbol, Decimal pipCost)
        {
            this.Name = name;
            this.Symbol = symbol;
            this._pipCost = pipCost;
            this.LastValue = double.NaN;
            this.Last = 0.0;
            this.Change = 0.0;
            this.ChangePercent = 0.0;
        }

        public void SetQuoteProvider(QuoteProvider quoteProvider)
        {
            this._quoteProvider = quoteProvider;
            this._quoteProvider.QuoteUpdated += new EventHandler<QuoteUpdatedEventArgs>(this.QuoteProviderQuoteUpdated);
            this._timer = new Timer(Convert.ToDouble((object)this.TimeFrame));
            this._timer.Elapsed += new ElapsedEventHandler(this.TimerElapsed);
        }

        public static TimeFrame ParseTimeFrame(string timeFrame)
        {
            switch (timeFrame)
            {
                case "M1":
                    return TimeFrame.M1;
                case "M5":
                    return TimeFrame.M5;
                case "M15":
                    return TimeFrame.M15;
                case "M30":
                    return TimeFrame.M30;
                case "H1":
                    return TimeFrame.H1;
                default:
                    throw new Exception(string.Format("Unknown time frame value '{0}'.", (object)timeFrame));
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isRunning)
                return;
            lock (this._locker)
            {
                try
                {
                    this.UpdateHistory(this._quoteProvider.GetHistory(this.Symbol, this.TimeFrame));
                    if (double.IsNaN(this.LastValue))
                    {
                        if (this.Last <= 0.0)
                            return;
                        this.LastValue = Convert.ToDouble(this.Last);
                    }
                    else
                    {
                        if (Math.Abs(this.Last - this.LastValue) > (double)(this.Range * this._pipCost))
                            this.RiseSignal(this.Last - this.LastValue > 0.0 ? SignalType.Call : SignalType.Put);
                        this.LastValue = this.Last;
                    }
                }
                catch (Exception exception_0)
                {
                    this._timer.Stop();
                    if (this.Error == null)
                        return;
                    this.Error((object)this, new InstrumentErrorEventArgs(exception_0.Message, this.Symbol));
                }
            }
        }

        private void RiseSignal(SignalType type)
        {
            if (this.RisedSignal == null)
                return;
            this.RisedSignal((object)this, new SignalEventArgs(type, this.Symbol, this.TimeFrame, this.Range, DateTime.UtcNow));
        }

        private void UpdateHistory(List<Candle> candles)
        {
            if (this.HistoryUpdated == null || candles.Count <= 0)
                return;
            this.HistoryUpdated((object)this, new HistoryEventArgs(this.Symbol, this.TimeFrame, candles));
        }

        public void QuoteProviderQuoteUpdated(object sender, QuoteUpdatedEventArgs e)
        {
            if (!(e.Symbol == this.Symbol))
                return;
            lock (this._locker)
            {
                this.Change = e.Change;
                this.ChangePercent = e.ChangePercent;
                this.Last = (e.Ask + e.Bid) / 2.0;
                if (!double.IsNaN(this.LastValue))
                    return;
                this.LastValue = this.Last;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void Start()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
            _timer.Start();
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
            _timer.Stop();
        }

        //protected void OnPropertyChanged(string name)
        //{
        //    PropertyChangedEventHandler changedEventHandler = this.PropertyChanged;
        //    if (changedEventHandler == null)
        //        return;
        //    changedEventHandler((object)this, new PropertyChangedEventArgs(name));
        //}

        public void Dispose()
        {
            if (_timer != null)
            {
                _isRunning = false;
                _disposed = true;
                _timer.Stop();
                _timer.Dispose();
            }
        }
    }
}
