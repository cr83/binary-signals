using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyBot.Models.Enums;

namespace EasyBot.Models
{
    public class Signal
    {
        public SignalType Type { get; private set; }

        public string Symbol { get; private set; }

        public TimeFrame TimeFrame { get; private set; }

        public Decimal Range { get; private set; }

        public DateTime DateTime { get; private set; }

        public string LocalTime
        {
            get
            {
                return this.DateTime.ToLocalTime().ToLongTimeString();
            }
        }

        public Signal(SignalType type, string symbol, TimeFrame timeFrame, Decimal range, DateTime dateTime)
        {
            this.Type = type;
            this.Symbol = symbol;
            this.TimeFrame = timeFrame;
            this.Range = range;
            this.DateTime = dateTime;
        }
    }
}
