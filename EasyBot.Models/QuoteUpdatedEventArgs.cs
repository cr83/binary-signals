using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBot.Models
{
    public class QuoteUpdatedEventArgs : EventArgs
    {
        public string Symbol { get; private set; }

        public double Ask { get; private set; }

        public double Bid { get; private set; }

        public double Change { get; private set; }

        public double ChangePercent { get; private set; }

        public QuoteUpdatedEventArgs(string symbol, double ask, double bid, double change, double changePercent)
        {
            this.Symbol = symbol;
            this.Ask = ask;
            this.Bid = bid;
            this.Change = change;
            this.ChangePercent = changePercent;
        }
    }
}
