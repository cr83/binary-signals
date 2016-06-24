using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyBot.Models.Enums;

namespace EasyBot.Models
{
    public class HistoryEventArgs : EventArgs
    {
        public string Symbol { get; private set; }

        public TimeFrame TimeFrame { get; private set; }

        public List<Candle> Candles { get; private set; }

        public HistoryEventArgs(string symbol, TimeFrame timeFrame, List<Candle> candles)
        {
            this.Symbol = symbol;
            this.TimeFrame = timeFrame;
            this.Candles = candles;
        }
    }
}
