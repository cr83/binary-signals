using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBot.Models
{
    public class Candle
    {
        public DateTime Date { get; private set; }

        public double Open { get; private set; }

        public double High { get; private set; }

        public double Low { get; private set; }

        public double Close { get; private set; }

        public Candle(double open, double high, double low, double close, DateTime dateTime)
        {
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Date = dateTime;
        }
    }
}
